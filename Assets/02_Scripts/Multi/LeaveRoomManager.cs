using UnityEngine;
using UnityEngine.UI;
using Photon.Pun; // Photon 관련 네임스페이스
using UnityEngine.SceneManagement;

public class LeaveRoomManager : MonoBehaviour
{
    // 떠나기 버튼을 연결할 수 있는 public 변수
    public Button leaveRoomButton;

    void Start()
    {
        // leaveRoomButton에 클릭 이벤트 리스너 추가
        leaveRoomButton.onClick.AddListener(OnLeaveRoomButtonClicked);
    }

    void OnLeaveRoomButtonClicked()
    {
        // Photon Room을 떠나는 기능
        PhotonNetwork.LeaveRoom();
        Debug.Log("Room을 떠났습니다.");

        SceneManager.LoadScene("01_Scenes/03CardGameVR/Lobby");
    }
}
