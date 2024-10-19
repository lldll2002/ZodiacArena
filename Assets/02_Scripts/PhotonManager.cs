using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    #region Settings
    //1. 게임 설정
    [Header("Game Settings")]
    [SerializeField] private const string version = "1.0";
    [SerializeField] private string nickName = "Sesac";

    //============================================================
    //2. Lobby UI 관련
    [Header("UI")]
    [SerializeField] private TMP_InputField nickNameIf;

    [Header("Button")]
    [SerializeField] private Button enterRoomButton;

    //============================================================
    //3. 룸 만들기
    [Header("Room")]
    [SerializeField] private TMP_InputField roomNameIf;
    [SerializeField] private Button makeRoomButton;

    //============================================================
    //4. Room List 만들기
    //[Header("Room List")]
    //public GameObject roomPrefab;
    //public Transform contentTr;
    //private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();

    #endregion

    //---------------------------------------------------------
    #region Awake & Start
    void Awake()
    {
        //1. 게임 버전 설정
        PhotonNetwork.GameVersion = version;
        //2. 유저명 설정
        PhotonNetwork.NickName = nickName;
        //3. 방장이 씬을 로딩했을 때, 클라이언트에 자동으로 해당 씬이 로딩되는 옵션
        PhotonNetwork.AutomaticallySyncScene = true;

        //4. 포톤 서버에 접속
        if (PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("NICK_NAME"))
        {
            nickName = PlayerPrefs.GetString("NICK_NAME");
            nickNameIf.text = nickName;
        }

        SetNickName();

        //버튼 이벤트 연결
        enterRoomButton.onClick.AddListener(() => OnLoginButtonClick());
        makeRoomButton.onClick.AddListener(() => OnMakeRoomButtonClick());
    }

    #endregion

    //---------------------------------------------------------
    #region MakeRoom
    private void OnMakeRoomButtonClick()
    {
        //닉네임 여부 확인
        SetNickName();

        // 룸 이름 입력 여부 확인
        if (string.IsNullOrEmpty(roomNameIf.text))
        {
            roomNameIf.text = $"ROOM_{Random.Range(0, 1000):0000}";
        }

        //룸 속성을 정의
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 6;
        ro.IsOpen = true;
        ro.IsVisible = true;

        //룸 생성
        PhotonNetwork.CreateRoom(roomNameIf.text, ro);

    }

    #endregion

    //---------------------------------------------------------
    #region Nickname & Login
    private void SetNickName()
    {
        //닉네임이 비어있는지 확인
        if (string.IsNullOrEmpty(nickNameIf.text))
        {
            nickName = $"USER_{Random.Range(0, 1000):0000}";
            nickNameIf.text = nickName;
        }

        nickName = nickNameIf.text;
        PhotonNetwork.NickName = nickName;
    }

    public void OnLoginButtonClick()
    {
        SetNickName();

        PlayerPrefs.SetString("NICK_NAME", nickName);
        PhotonNetwork.JoinRandomRoom();
    }

    #endregion

    //---------------------------------------------------------
    #region Photon Network

    //1. 포톤 서버에 접속되었을 때 호출되는 콜백
    public override void OnConnectedToMaster()
    {
        Debug.Log("서버 접속 완료");
        PhotonNetwork.JoinLobby();
    }

    //2. 로비에 입장했을 때 호출되는 콜백
    public override void OnJoinedLobby()
    {
        Debug.Log("로비 입장 완료");
    }

    //3. 랜덤 방입장 실패시 호출되는 콜백
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"방 입장 실패: {message}");

        //방 생성시 룸 옵션
        RoomOptions ro = new RoomOptions
        {
            MaxPlayers = 2,
            IsOpen = true,
            IsVisible = true
        };

        PhotonNetwork.CreateRoom("MyRoom" + Random.Range(0, 100), ro);
    }

    //4. 방생성 완료시 콜백
    public override void OnCreatedRoom()
    {
        Debug.Log("방 생성 완료");
    }

    //5. 방 입장 후 콜백
    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 완료");

        //전투 씬 로딩
        //방장만 로딩하도록 설정.
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("01_Scenes/Game");
        }
    }

    #endregion

    //---------------------------------------------------------
    /*
    #region Room List
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var room in roomList)
        {
            Debug.Log($"{room.Name} {room.PlayerCount} / {room.MaxPlayers}");

            // 삭제된 룸
            if (room.RemovedFromList == true)
            {
                // 룸 삭제
                if (roomDict.TryGetValue(room.Name, out GameObject tempRoom))
                {
                    //room 프리팹 클론을 삭제
                    Destroy(tempRoom);

                    // 딕셔너리의 레코드를 삭제
                    roomDict.Remove(room.Name);
                }
                continue;
            }


            //새로 생성된 룸, 변경된 경우
            if (roomDict.ContainsKey(room.Name) == false)
            {
                //처음 생성된 룸
                var _room = Instantiate(roomPrefab, contentTr);

                // RoomPrefab에 RoomInfo 값을 저장
                _room.GetComponent<RoomData>().RoomInfo = room;

                //딕셔너리에 저장
                roomDict.Add(room.Name, _room);
            }
            else
            {
                // 이전에 생성되었던 룸
                // 룸 정보를 갱신
                if (roomDict.TryGetValue(room.Name, out GameObject tempRoom))
                {
                    tempRoom.GetComponent<RoomData>().RoomInfo = room;
                }
            }
        }
    }
    #endregion
*/

}
