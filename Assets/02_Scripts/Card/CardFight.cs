using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager를 사용하기 위해 추가
using UnityEngine.UI; // Button 사용을 위해 추가
using System.Collections; // IEnumerator 사용을 위해 추가

public class CardFight : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    [SerializeField] private TMP_Text playerNamesText; // 플레이어 vs. 플레이어 닉네임 표시 텍스트
    [SerializeField] private TMP_Text winConditionText; // 승리 조건 표시 텍스트

    [Header("Player Info")]
    [SerializeField] private TMP_Text player1NameText; // 플레이어 1 이름 표시 텍스트
    [SerializeField] private TMP_Text player2NameText; // 플레이어 2 이름 표시 텍스트
    [SerializeField] private TMP_Text player1CardText; // 플레이어 1 선택한 카드 텍스트
    [SerializeField] private TMP_Text player2CardText; // 플레이어 2 선택한 카드 텍스트
    [SerializeField] private TMP_Text resultText; // 승리 결과 표시 텍스트
    [SerializeField] private Button nextButton; // 버튼 변수 추가

    [Header("Model summon")]
    [SerializeField] private Transform player1SpawnPoint; // 플레이어 1의 별자리 모델이 생성될 위치
    [SerializeField] private Transform player2SpawnPoint; // 플레이어 2의 별자리 모델이 생성될 위치
    [SerializeField] private GameObject[] zodiacPrefabs; // 1~12 사이의 별자리 모델을 담은 배열 (별자리 프리팹)

    [Header("Effects")]
    [SerializeField] private GameObject visualEffectPrefab; // 비주얼 이펙트 프리팹
    [SerializeField] private GameObject collisionEffectPrefab; // 부딪힘 효과 프리팝
    [SerializeField] private GameObject winEffectPrefab; // 승리 이펙트 프리팹

    private int player1Card; // 플레이어 1 선택한 카드
    private int player2Card; // 플레이어 2 선택한 카드

    private bool hasClicked = false; // 중복 클릭 방지 변수

    private GameObject player1ZodiacInstance; // 플레이어 1의 별자리 모델
    private GameObject player2ZodiacInstance; // 플레이어 2의 별자리 모델
    private GameObject visualEffectInstance; // 비주얼 이펙트 인스턴스

    void Start()
    {
        // 유저명 및 승리 조건 초기화
        UpdatePlayerInfo();

        // UI 요소 비활성화
        playerNamesText.gameObject.SetActive(false);
        winConditionText.gameObject.SetActive(false);
        player1NameText.gameObject.SetActive(false);
        player2NameText.gameObject.SetActive(false);
        player1CardText.gameObject.SetActive(false);
        player2CardText.gameObject.SetActive(false);
        resultText.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false); // 다음 버튼 비활성화

        // 다음 버튼 클릭 이벤트 리스너 추가
        nextButton.onClick.AddListener(OnNextButtonClicked);

        // 컷씬 시작
        StartCoroutine(PlayCutscene());
    }

    private void OnNextButtonClicked()
    {
        // Photon Room을 나가고 FightResult 씬으로 이동
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        // 방을 나갔을 때 FightResult 씬으로 이동
        SceneManager.LoadScene("01_Scenes/03CardGameVR/FightResult");
    }

    private void UpdatePlayerInfo()
    {
        // 플레이어 닉네임을 "플레이어1 vs. 플레이어2" 형식으로 표시
        string player1Name = PhotonNetwork.PlayerList[0].NickName;
        string player2Name = PhotonNetwork.PlayerList[1].NickName;
        playerNamesText.text = $"{player1Name} vs. {player2Name}"; // 플레이어 이름 표시

        // 각 플레이어 이름을 카드 텍스트 위에 표시
        player1NameText.text = player1Name; // 플레이어 1 이름 표시
        player2NameText.text = player2Name; // 플레이어 2 이름 표시

        // 카드 정보를 CustomProperties에서 가져와서 표시
        if (PhotonNetwork.PlayerList[0].CustomProperties.ContainsKey("selectedCard"))
        {
            player1Card = (int)PhotonNetwork.PlayerList[0].CustomProperties["selectedCard"];
            player1CardText.text = $"Selected Card: {player1Card}"; // 플레이어 1 카드 표시
        }

        if (PhotonNetwork.PlayerList[1].CustomProperties.ContainsKey("selectedCard"))
        {
            player2Card = (int)PhotonNetwork.PlayerList[1].CustomProperties["selectedCard"];
            player2CardText.text = $"Selected Card: {player2Card}"; // 플레이어 2 카드 표시
        }

        // 승리 조건 텍스트 초기화
        winConditionText.text = PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("winCondition")
            ? (string)PhotonNetwork.LocalPlayer.CustomProperties["winCondition"]
            : "High"; // 기본값 설정
    }

    private IEnumerator PlayCutscene()
    {
        // 플레이어 1의 카드 모델 생성 및 비주얼 이펙트 실행
        yield return StartCoroutine(SpawnZodiacModel(player1Card, player1SpawnPoint, instance => player1ZodiacInstance = instance));
        yield return new WaitForSeconds(1f); // 모델 생성 후 대기

        // 플레이어 2의 카드 모델 생성 및 비주얼 이펙트 실행
        yield return StartCoroutine(SpawnZodiacModel(player2Card, player2SpawnPoint, instance => player2ZodiacInstance = instance));
        yield return new WaitForSeconds(1f); // 모델 생성 후 대기

        // 비주얼 이펙트 생성 (부딪히기 전)
        visualEffectInstance = Instantiate(visualEffectPrefab, (player1ZodiacInstance.transform.position + player2ZodiacInstance.transform.position) / 2, Quaternion.identity);

        // 두 모델이 중간 지점으로 이동
        Vector3 midpoint = (player1ZodiacInstance.transform.position + player2ZodiacInstance.transform.position) / 2;
        float speed = 1f; // 속도 조절

        // 두 모델이 중간 지점으로 이동
        while (Vector3.Distance(player1ZodiacInstance.transform.position, midpoint) > 0.1f || Vector3.Distance(player2ZodiacInstance.transform.position, midpoint) > 0.1f)
        {
            player1ZodiacInstance.transform.position = Vector3.MoveTowards(player1ZodiacInstance.transform.position, midpoint, speed * Time.deltaTime);
            player2ZodiacInstance.transform.position = Vector3.MoveTowards(player2ZodiacInstance.transform.position, midpoint, speed * Time.deltaTime);
            yield return null; // 한 프레임 대기
        }

        // 부딪히는 효과 생성
        GameObject collisionEffectInstance = Instantiate(collisionEffectPrefab, midpoint, Quaternion.identity); // 부딪힘 이펙트 생성
        Destroy(collisionEffectInstance, 0.1f); // 0.1초 후 이펙트 삭제

        // 2초 대기 (부딪힘 효과 지속 시간)
        yield return new WaitForSeconds(2f);

        // 비주얼 이펙트 삭제
        Destroy(visualEffectInstance); // 비주얼 이펙트 삭제

        // 승자에 따라 쓰러지는 연출
        if (player1Card > player2Card)
        {
            player2ZodiacInstance.transform.Rotate(-90, 0, 0); // 플레이어 2가 패배
            player2ZodiacInstance.transform.position += new Vector3(0, -1, 0); // 패배자는 아래로 이동
            Instantiate(winEffectPrefab, player1ZodiacInstance.transform.position, Quaternion.identity); // 승리 이펙트 생성
            resultText.text = $"{PhotonNetwork.PlayerList[0].NickName} wins with {player1Card}!";
        }
        else if (player1Card < player2Card)
        {
            player1ZodiacInstance.transform.Rotate(-90, 0, 0); // 플레이어 1이 패배
            player1ZodiacInstance.transform.position += new Vector3(0, -1, 0); // 패배자는 아래로 이동
            Instantiate(winEffectPrefab, player2ZodiacInstance.transform.position, Quaternion.identity); // 승리 이펙트 생성
            resultText.text = $"{PhotonNetwork.PlayerList[1].NickName} wins with {player2Card}!";
        }
        else
        {
            resultText.text = "It's a tie!";
        }

        // 컷씬이 끝난 후 UI 활성화
        yield return new WaitForSeconds(5f); // 쓰러짐 연출 대기
        playerNamesText.gameObject.SetActive(true);
        winConditionText.gameObject.SetActive(true);
        player1NameText.gameObject.SetActive(true);
        player2NameText.gameObject.SetActive(true);
        player1CardText.gameObject.SetActive(true);
        player2CardText.gameObject.SetActive(true);
        resultText.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(true); // 다음 버튼 활성화
    }

    private IEnumerator SpawnZodiacModel(int card, Transform spawnPoint, System.Action<GameObject> onInstantiate)
    {
        // 카드에 해당하는 별자리 모델 프리팹 생성
        if (card >= 1 && card <= zodiacPrefabs.Length)
        {
            GameObject instance = Instantiate(zodiacPrefabs[card - 1], spawnPoint.position, Quaternion.identity); // 카드에 해당하는 모델 생성
            yield return null; // 프레임 대기
            onInstantiate(instance); // 생성된 인스턴스 반환
        }
        else
        {
            Debug.LogWarning("Invalid card number: " + card);
        }
    }
}
