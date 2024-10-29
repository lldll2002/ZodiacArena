using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;

public class FightCutScene : MonoBehaviourPunCallbacks
{
    [Header("Model summon")]
    [SerializeField] private Transform player1SpawnPoint;
    [SerializeField] private Transform player2SpawnPoint;
    [SerializeField] private GameObject[] zodiacPrefabs;
    public float moveDuration = 2.0f;

    [Header("Effects")]
    [SerializeField] private GameObject visualEffectPrefab;
    [SerializeField] private GameObject collisionEffectPrefab;
    [SerializeField] private GameObject winEffectPrefab;

    private int player1Card;
    private int player2Card;

    private GameObject player1ZodiacInstance;
    private GameObject player2ZodiacInstance;
    private GameObject visualEffectInstance;

    private AudioSource audioSource;
    [SerializeField] AudioClip spwanEffectSound;
    [SerializeField] AudioClip fightEffectSound;

    private string winCondition;

    void Start()
    {
        // AudioSource 초기화 및 없으면 수동으로 추가
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing.");
        }

        UpdatePlayerInfo();

        // 승리 조건을 PlayerPrefs에서 불러오기
        winCondition = PlayerPrefs.GetString("WinCondition", "DefaultCondition");
    }

    private void UpdatePlayerInfo()
    {
        // 카드 정보를 CustomProperties에서 가져와서 업데이트
        if (PhotonNetwork.PlayerList.Length > 0 && PhotonNetwork.PlayerList[0].CustomProperties.ContainsKey("selectedCard"))
        {
            player1Card = (int)PhotonNetwork.PlayerList[0].CustomProperties["selectedCard"];
        }

        if (PhotonNetwork.PlayerList.Length > 1 && PhotonNetwork.PlayerList[1].CustomProperties.ContainsKey("selectedCard"))
        {
            player2Card = (int)PhotonNetwork.PlayerList[1].CustomProperties["selectedCard"];
        }

        // 카드 정보가 업데이트되었을 때 애니메이션 시작
        StartCoroutine(ModelTransform());
    }

    private IEnumerator ModelTransform()
    {
        // 1번 플레이어 이펙트 소환
        visualEffectInstance = Instantiate(visualEffectPrefab, player1SpawnPoint.position, Quaternion.identity);
        SpawnEffectSound();
        Destroy(visualEffectInstance, 1.0f); // 이펙트를 1초 후에 삭제
        yield return new WaitForSeconds(0.5f); // 약간의 지연 후 모델 소환

        // 1번 플레이어의 별자리 모델 소환
        player1ZodiacInstance = SpawnZodiacModel(player1Card, player1SpawnPoint);
        if (player1ZodiacInstance != null)
        {
            player1ZodiacInstance.transform.localScale = Vector3.zero;
            player1ZodiacInstance.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
            yield return player1ZodiacInstance.transform.DORotate(new Vector3(0, 540, 0), 2.0f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear).WaitForCompletion();
        }

        // 2번 플레이어 이펙트 소환
        visualEffectInstance = Instantiate(visualEffectPrefab, player2SpawnPoint.position, Quaternion.identity);
        SpawnEffectSound();
        Destroy(visualEffectInstance, 1.0f);
        yield return new WaitForSeconds(0.5f);

        // 2번 플레이어의 별자리 모델 소환
        player2ZodiacInstance = SpawnZodiacModel(player2Card, player2SpawnPoint);
        if (player2ZodiacInstance != null)
        {
            player2ZodiacInstance.transform.localScale = Vector3.zero;
            player2ZodiacInstance.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
            yield return player2ZodiacInstance.transform.DORotate(new Vector3(0, 540, 0), 2.0f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear).WaitForCompletion();
        }

        // 두 오브젝트가 중간 지점으로 이동하도록 설정
        Vector3 midpoint = (player1SpawnPoint.position + new Vector3(0, 1.0f, 0) + player2SpawnPoint.position + new Vector3(0, 1.0f, 0)) / 2;
        player1ZodiacInstance.transform.DOMove(midpoint, moveDuration).SetEase(Ease.Linear);
        player2ZodiacInstance.transform.DOMove(midpoint, moveDuration).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1.5f);

        // 부딪힘 효과(shake)
        player1ZodiacInstance.transform.DOShakePosition(0.5f, 1, 10, 50, false, false);
        player2ZodiacInstance.transform.DOShakePosition(0.5f, 1, 10, 50, false, false);
        FightEffectSound();

        GameObject collisionEffectInstance = Instantiate(collisionEffectPrefab, midpoint, Quaternion.identity);
        Destroy(collisionEffectInstance, 0.1f);

        // 2초 대기 (부딪힘 효과 지속 시간)
        yield return new WaitForSeconds(2f);

        // 두 모델 삭제
        Destroy(player1ZodiacInstance);
        Destroy(player2ZodiacInstance);
        Destroy(visualEffectInstance);

        CheckWinCondition();

        // 컷씬이 끝난 후 방 나가기 및 다음 씬으로 이동
        PhotonNetwork.LeaveRoom();
    }

    private void CheckWinCondition()
    {
        int localPlayerCard = (int)PhotonNetwork.LocalPlayer.CustomProperties["selectedCard"];
        int opponentCard = (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[0]) ? player2Card : player1Card;
        int winner;

        if (winCondition == "High")
        {
            winner = (localPlayerCard > opponentCard) ? 1 : (localPlayerCard < opponentCard) ? 2 : 0;
        }
        else
        {
            winner = (localPlayerCard < opponentCard) ? 1 : (localPlayerCard > opponentCard) ? 2 : 0;
        }

        PlayerPrefs.SetInt("PlayerWon", winner == 1 ? 1 : winner == 0 ? -1 : 0);
        PlayerPrefs.Save();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("selectedCard"))
        {
            UpdatePlayerInfo();
            CheckWinCondition();
        }
    }


    public override void OnLeftRoom()
    {
        // 방을 나갔을 때 FightResult 씬으로 이동
        SceneManager.LoadScene("01_Scenes/02CardGameVR/FightResult");
    }

    private GameObject SpawnZodiacModel(int card, Transform spawnPoint)
    {
        if (card >= 1 && card <= zodiacPrefabs.Length)
        {
            GameObject instance = Instantiate(zodiacPrefabs[card - 1], spawnPoint.position, Quaternion.identity);
            return instance;
        }
        else
        {
            Debug.LogWarning("Invalid card number: " + card);
            return null;
        }
    }

    private void SpawnEffectSound()
    {
        if (spwanEffectSound != null)
        {
            audioSource.PlayOneShot(spwanEffectSound);
        }
        else
        {
            Debug.LogWarning("Spawn effect sound clip is missing.");
        }
    }

    private void FightEffectSound()
    {
        if (fightEffectSound != null)
        {
            audioSource.PlayOneShot(fightEffectSound);
        }
        else
        {
            Debug.LogWarning("Fight effect sound clip is missing.");
        }
    }
}
