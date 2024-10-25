using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CheckRoomManager : MonoBehaviourPunCallbacks
{
    public GameObject leaveRoomCanvas; // LeaveRoomCanvas GameObject
    public TMP_Text notificationText; // 안내 문구를 표시할 Text UI 요소

    void Update()
    {
        CheckPlayerCount();
    }

    // 룸의 플레이어 수를 체크하는 함수
    private void CheckPlayerCount()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2) // 플레이어가 2명 미만일 경우
        {
            ShowLeaveRoomCanvas(); // LeaveRoomCanvas 활성화
        }
        else
        {
            leaveRoomCanvas.SetActive(false); // 플레이어 수가 2명 이상일 경우 비활성화
        }
    }

    // LeaveRoomCanvas를 활성화하고 안내 문구 표시
    private void ShowLeaveRoomCanvas()
    {
        // 모든 다른 캔버스를 비활성화
        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.gameObject != leaveRoomCanvas) // 현재 LeaveRoomCanvas가 아닐 경우 비활성화
            {
                canvas.gameObject.SetActive(false);
            }
        }

        leaveRoomCanvas.SetActive(true); // LeaveRoomCanvas 활성화
        notificationText.text = "상대방 플레이어가 룸을 떠났습니다.(곧 세션이 종료됩니다.)"; // 안내 문구 설정
        StartCoroutine(LeaveRoomAfterDelay(3f)); // 3초 후 LeaveRoom 호출
    }

    // 지정된 시간 후에 룸을 떠나는 코루틴
    private IEnumerator LeaveRoomAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.LeaveRoom(); // 룸 떠나기
    }
}
