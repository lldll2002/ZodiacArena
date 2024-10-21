using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonGameManager : MonoBehaviourPunCallbacks
{
    #region Settings
    // 1. 게임 설정
    [Header("Game Settings")]
    [SerializeField] private const string version = "1.0";
    [SerializeField] private string nickName = "Sesac";

    //============================================================
    // 2. Lobby UI 관련
    [Header("UI")]
    [SerializeField] private TMP_InputField nickNameIf;

    [Header("Button")]
    [SerializeField] private Button enterRoomButton;

    //============================================================
    // 3. 룸 만들기
    [Header("Room")]
    [SerializeField] private TMP_InputField roomNameIf;
    [SerializeField] private Button makeRoomButton;

    #endregion

    private LoginManager loginManager;

    //---------------------------------------------------------
    #region Awake & Start
    void Awake()
    {
        // 1. 게임 버전 설정
        PhotonNetwork.GameVersion = version;
        // 2. 유저명 설정
        PhotonNetwork.NickName = nickName;
        // 3. 방장이 씬을 로딩했을 때, 클라이언트에 자동으로 해당 씬이 로딩되는 옵션
        PhotonNetwork.AutomaticallySyncScene = true;

        // 4. 포톤 서버에 접속
        if (PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    void Start()
    {
        // LoginManager를 찾아서 저장
        loginManager = FindObjectOfType<LoginManager>();

        if (PlayerPrefs.HasKey("NICK_NAME"))
        {
            nickName = PlayerPrefs.GetString("NICK_NAME");
            nickNameIf.text = nickName;
        }

        SetNickName();

        // 버튼 이벤트 연결
        enterRoomButton.onClick.AddListener(() => OnLoginButtonClick());
        makeRoomButton.onClick.AddListener(() => OnMakeRoomButtonClick());
    }
    #endregion

    //---------------------------------------------------------
    #region Nickname & Login
    private void SetNickName()
    {
        // 닉네임이 비어 있는지 확인
        if (string.IsNullOrEmpty(nickNameIf.text))
        {
            nickName = $"USER_{Random.Range(0, 1000):0000}";
            nickNameIf.text = nickName;
        }

        nickName = nickNameIf.text;
        PhotonNetwork.NickName = nickName;

        // 닉네임을 Cloud Save에 업데이트
        loginManager?.UpdatePlayerNickName(nickName);
    }

    public void OnLoginButtonClick()
    {
        SetNickName();

        PlayerPrefs.SetString("NICK_NAME", nickName);
        PhotonNetwork.JoinRandomRoom();
    }
    #endregion

    //---------------------------------------------------------
    #region MakeRoom
    private void OnMakeRoomButtonClick()
    {
        // 닉네임 여부 확인
        SetNickName();

        // 룸 이름 입력 여부 확인
        if (string.IsNullOrEmpty(roomNameIf.text))
        {
            roomNameIf.text = $"ROOM_{Random.Range(0, 1000):0000}";
        }

        // 룸 속성을 정의
        RoomOptions ro = new RoomOptions
        {
            MaxPlayers = 2,
            IsOpen = true,
            IsVisible = true
        };

        // 룸 생성
        PhotonNetwork.CreateRoom(roomNameIf.text, ro);
    }
    #endregion

    //---------------------------------------------------------
    #region Photon Network
    // 1. 포톤 서버에 접속되었을 때 호출되는 콜백
    public override void OnConnectedToMaster()
    {
        Debug.Log("서버 접속 완료");
        PhotonNetwork.JoinLobby();
    }

    // 2. 로비에 입장했을 때 호출되는 콜백
    public override void OnJoinedLobby()
    {
        Debug.Log("로비 입장 완료");
    }

    // 3. 랜덤 방입장 실패 시 호출되는 콜백
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"방 입장 실패: {message}");

        // 방 생성 시 룸 옵션
        RoomOptions ro = new RoomOptions
        {
            MaxPlayers = 2,
            IsOpen = true,
            IsVisible = true
        };

        PhotonNetwork.CreateRoom("MyRoom" + Random.Range(0, 100), ro);
    }

    // 4. 방 생성 완료 시 콜백
    public override void OnCreatedRoom()
    {
        Debug.Log("방 생성 완료");
    }

    // 5. 방 입장 후 콜백
    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 완료");

        // 전투 씬 로딩
        // 방장만 로딩하도록 설정
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("01_Scenes/03CardGameVR/RoomEnter");
        }
    }
    #endregion
}
