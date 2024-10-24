using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager를 사용하기 위해 추가
using UnityEngine.UI; // Button 사용을 위해 추가

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

    private int player1Card; // 플레이어 1 선택한 카드
    private int player2Card; // 플레이어 2 선택한 카드

    private bool hasClicked = false; // 중복 클릭 방지 변수

    void Start()
    {
        // 유저명 및 승리 조건 초기화
        UpdatePlayerInfo();

        // 별자리 모델 스폰
        SpawnZodiacModels();

        // 승리 조건 확인
        CheckWinner();

        // 버튼 클릭 이벤트 연결
        nextButton.onClick.AddListener(OnClickToFightResult);
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

    private void SpawnZodiacModels()
    {
        // player1Card에 해당하는 별자리 프리팹 생성 (1~12 사이 값)
        if (player1Card >= 1 && player1Card <= 12)
        {
            GameObject player1ZodiacPrefab = zodiacPrefabs[player1Card - 1]; // 배열이 0부터 시작하므로 -1
            Instantiate(player1ZodiacPrefab, player1SpawnPoint.position, player1SpawnPoint.rotation);
        }

        // player2Card에 해당하는 별자리 프리팹 생성 (1~12 사이 값)
        if (player2Card >= 1 && player2Card <= 12)
        {
            GameObject player2ZodiacPrefab = zodiacPrefabs[player2Card - 1]; // 배열이 0부터 시작하므로 -1
            Instantiate(player2ZodiacPrefab, player2SpawnPoint.position, player2SpawnPoint.rotation);
        }
    }

    private void CheckWinner()
    {
        // 하이라면 높은 숫자를 선택한 플레이어가 승리
        // 로우라면 낮은 숫자를 선택한 플레이어가 승리
        if (player1Card > player2Card)
        {
            resultText.text = $"{PhotonNetwork.PlayerList[0].NickName} wins with {player1Card}!";
            SetPlayerWinStatus(true, false); // 플레이어 1 승리
        }
        else if (player1Card < player2Card)
        {
            resultText.text = $"{PhotonNetwork.PlayerList[1].NickName} wins with {player2Card}!";
            SetPlayerWinStatus(false, true); // 플레이어 2 승리
        }
        else
        {
            resultText.text = "It's a tie!";
            SetPlayerWinStatus(false, false); // 무승부
        }
    }

    private void SetPlayerWinStatus(bool player1Won, bool player2Won)
    {
        // 현재 플레이어가 플레이어 1인지 2인지 확인
        if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[0])
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
            {
                { "playerWon", player1Won },
            });
            // 플레이어 2 승리 여부 저장
            PhotonNetwork.PlayerList[1].SetCustomProperties(new ExitGames.Client.Photon.Hashtable
            {
                { "playerWon", player2Won },
            });
        }
        else
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
            {
                { "playerWon", player2Won },
            });
            // 플레이어 1 승리 여부 저장
            PhotonNetwork.PlayerList[0].SetCustomProperties(new ExitGames.Client.Photon.Hashtable
            {
                { "playerWon", player1Won },
            });
        }
    }

    private void OnClickToFightResult()
    {
        // playerWon 값을 안전하게 가져오기
        bool playerWon = false;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("playerWon", out object value) && value is bool)
        {
            playerWon = (bool)value;
        }

        // 승리 여부를 PlayerPrefs에 저장
        PlayerPrefs.SetInt("PlayerWon", playerWon ? 1 : 0); // 이겼으면 1, 졌으면 0

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LeaveRoom(); // 방을 떠남
            // LeaveRoom의 콜백을 기다렸다가 씬 전환
            Invoke("LoadFightResultScene", 0.5f); // 0.5초 후에 씬 전환
        }
    }

    private void LoadFightResultScene()
    {
        SceneManager.LoadScene("01_Scenes/03CardGameVR/FightResult");
    }
}