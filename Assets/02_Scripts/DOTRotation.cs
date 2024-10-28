using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using TMPro;


public class DOTRotation : MonoBehaviourPunCallbacks
{
    [Header("Model summon")]
    [SerializeField] private Transform player1SpawnPoint; // 플레이어 1의 별자리 모델이 생성될 위치
    [SerializeField] private Transform player2SpawnPoint; // 플레이어 2의 별자리 모델이 생성될 위치
    [SerializeField] private GameObject[] zodiacPrefabs; // 1~12 사이의 별자리 모델을 담은 배열 (별자리 프리팹)
    public float moveDuration = 2.0f; // 이동 시간

    [Header("Effects")]
    [SerializeField] private GameObject visualEffectPrefab; // 비주얼 이펙트 프리팹
    [SerializeField] private GameObject collisionEffectPrefab; // 부딪힘 효과 프리팝
    [SerializeField] private GameObject winEffectPrefab; // 승리 이펙트 프리팹

    private int player1Card; // 플레이어 1 선택한 카드
    private int player2Card; // 플레이어 2 선택한 카드

    private GameObject player1ZodiacInstance; // 플레이어 1의 별자리 모델
    private GameObject player2ZodiacInstance; // 플레이어 2의 별자리 모델
    private GameObject visualEffectInstance; // 비주얼 이펙트 인스턴스

    void Start()
    {
        StartCoroutine(ModelTransform());
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        // 플레이어 속성 업데이트가 감지되면 카드 정보를 업데이트
        UpdatePlayerInfo();
    }
    private IEnumerator ModelTransform()
    {

        // 1번 플레이어의 별자리 모델 소환 (예: 카드 번호 1, player1이 선택한 카드)
        player1ZodiacInstance = SpawnZodiacModel(player1Card, player1SpawnPoint);
        if (player1ZodiacInstance != null)
        {
            player1ZodiacInstance.transform.localScale = Vector3.zero;
            player1ZodiacInstance.transform.DOScale(Vector3.one, 1.0f).SetEase(Ease.OutBounce);
            yield return player1ZodiacInstance.transform.DORotate(new Vector3(0, 1270, 0), 1.0f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear).WaitForCompletion();
        }

        // 2번 플레이어의 별자리 모델 소환 (예: 카드 번호 2, player2가 선택한 카드)
        player2ZodiacInstance = SpawnZodiacModel(player2Card, player2SpawnPoint);
        if (player2ZodiacInstance != null)
        {
            player2ZodiacInstance.transform.localScale = Vector3.zero;
            player2ZodiacInstance.transform.DOScale(Vector3.one, 1.0f).SetEase(Ease.OutBounce);
            yield return player2ZodiacInstance.transform.DORotate(new Vector3(0, 1270, 0), 1.5f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear).WaitForCompletion();
        }




        // 두 오브젝트가 중간 지점으로 이동하도록 설정
        Vector3 midpoint = (player1SpawnPoint.position + new Vector3(0, 1.0f, 0) + player2SpawnPoint.position + new Vector3(0, 1.0f, 0)) / 2;

        // 첫 번째 오브젝트를 중간 지점으로 이동
        player1ZodiacInstance.transform.DOMove(midpoint, moveDuration).SetEase(Ease.Linear);

        // 두 번째 오브젝트를 중간 지점으로 이동
        player2ZodiacInstance.transform.DOMove(midpoint, moveDuration).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1.5f);

        // 부딪힘 효과(shake)
        player1ZodiacInstance.transform.DOShakePosition(0.5f, 1, 10, 50, false, false);
        player2ZodiacInstance.transform.DOShakePosition(0.5f, 1, 10, 50, false, false);
        yield return new WaitForSeconds(1.5f);
        GameObject collisionEffectInstance = Instantiate(collisionEffectPrefab, midpoint, Quaternion.identity); // 부딪힘 이펙트 생성
        Destroy(collisionEffectInstance, 0.1f); // 0.1초 후 이펙트 삭제



        // 2초 대기 (부딪힘 효과 지속 시간)
        yield return new WaitForSeconds(2f);

        // 두 모델 삭제
        Destroy(player1ZodiacInstance);
        Destroy(player2ZodiacInstance);
        // 비주얼 이펙트 삭제
        Destroy(visualEffectInstance); // 비주얼 이펙트 삭제

    }

    public void UpdatePlayerInfo()
    {
        // 카드 정보를 CustomProperties에서 가져와서 표시
        if (PhotonNetwork.PlayerList[0].CustomProperties.ContainsKey("selectedCard"))
        {
            player1Card = (int)PhotonNetwork.PlayerList[0].CustomProperties["selectedCard"];
        }

        if (PhotonNetwork.PlayerList[1].CustomProperties.ContainsKey("selectedCard"))
        {
            player2Card = (int)PhotonNetwork.PlayerList[1].CustomProperties["selectedCard"];
        }
    }

    private GameObject SpawnZodiacModel(int card, Transform spawnPoint)
    {
        // 카드에 해당하는 별자리 모델 프리팹 생성
        if (card >= 1 && card <= zodiacPrefabs.Length)
        {
            GameObject instance = Instantiate(zodiacPrefabs[card - 1], spawnPoint.position, Quaternion.identity); // 카드에 해당하는 모델 생성
            return instance; // 생성된 인스턴스를 반환
        }
        else
        {
            Debug.LogWarning("Invalid card number: " + card);
            return null;
        }
    }
}
