using Photon.Pun;
using TMPro; // TMP_Text 사용을 위해 추가
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BeforeGame : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text resultText; // 동전 던지기 결과 텍스트
    [SerializeField] private TMP_Text selectionText; // 선택 중 플레이어 이름을 표시할 텍스트
    [SerializeField] private TMP_Text playerNamesText; // 플레이어 이름 표시할 텍스트
    [SerializeField] private Button highButton; // 하이 선택 버튼
    [SerializeField] private Button lowButton; // 로우 선택 버튼

    private Photon.Realtime.Player currentPlayer; // 현재 턴을 가진 플레이어

    private void Start()
    {
        // PhotonView가 정상적으로 초기화되었는지 확인
        if (GetComponent<PhotonView>() == null)
        {
            Debug.LogError("PhotonView is not attached to this GameObject.");
            return;
        }

        // UI 요소들이 모두 할당되었는지 확인
        if (resultText == null || selectionText == null || playerNamesText == null || highButton == null || lowButton == null)
        {
            Debug.LogError("One or more UI elements are not assigned in the inspector.");
            return;
        }

        highButton.onClick.AddListener(() => OnWinConditionSelected("High"));
        lowButton.onClick.AddListener(() => OnWinConditionSelected("Low"));

        // 하이, 로우 버튼 비활성화
        highButton.gameObject.SetActive(false);
        lowButton.gameObject.SetActive(false);

        // 플레이어 이름 표시
        UpdatePlayerNames();

        // 게임 시작 시 자동으로 동전 던지기
        StartCoinFlip();
    }

    private void UpdatePlayerNames()
    {
        // 현재 로컬 플레이어와 상대 플레이어의 이름을 표시
        string otherPlayerNickName = PhotonNetwork.PlayerList[0].NickName == PhotonNetwork.LocalPlayer.NickName
            ? PhotonNetwork.PlayerList[1].NickName
            : PhotonNetwork.PlayerList[0].NickName;

        playerNamesText.text = $"{PhotonNetwork.LocalPlayer.NickName} vs. {otherPlayerNickName}";
    }

    private void StartCoinFlip()
    {
        // 동전 던지기 결과를 랜덤으로 결정
        bool isHeads = Random.Range(0, 2) == 0; // 0: 앞면, 1: 뒷면
        string result = isHeads ? "Heads" : "Tails";
        resultText.text = $"Coin flipped: {result}"; // TMP_Text에 결과 표시

        // 턴 할당
        currentPlayer = isHeads ? PhotonNetwork.LocalPlayer : GetOtherPlayer();
        AssignTurn(currentPlayer);
    }

    private Photon.Realtime.Player GetOtherPlayer()
    {
        return PhotonNetwork.PlayerList[0].NickName == PhotonNetwork.LocalPlayer.NickName
            ? PhotonNetwork.PlayerList[1]
            : PhotonNetwork.PlayerList[0];
    }

    private void AssignTurn(Photon.Realtime.Player player)
    {
        Debug.Log($"{player.NickName} is assigned to go first.");

        // 선택 권한이 있는 플레이어에게 하이, 로우 버튼 활성화
        if (player.IsLocal)
        {
            highButton.gameObject.SetActive(true);
            lowButton.gameObject.SetActive(true);
        }
        else
        {
            // 다른 플레이어에게 선택 중인 플레이어 표시
            selectionText.text = $"{player.NickName} is selecting High/Low";
        }
    }

    private void OnWinConditionSelected(string condition)
    {
        // 승리 조건을 다른 플레이어에게 전파
        photonView.RPC("NotifyWinCondition", RpcTarget.All, condition);

        // 버튼 비활성화
        highButton.gameObject.SetActive(false);
        lowButton.gameObject.SetActive(false);

        // 선택 후 텍스트 업데이트
        selectionText.text = $"Selected win condition: {condition}";

        // 모든 플레이어가 선택했는지를 확인
        StartCoroutine(LoadCardSelectAfterDelay(3f)); // 3초 대기 후 CardSelect 씬으로 이동
    }

    [PunRPC]
    private void NotifyWinCondition(string condition, PhotonMessageInfo info)
    {
        // 이 부분에서 info.Sender를 통해 어떤 플레이어가 선택했는지 확인할 수 있습니다.
        Debug.Log($"{info.Sender.NickName} selected win condition: {condition}");
        selectionText.text = $"{info.Sender.NickName} selected {condition}"; // 선택한 조건을 표시

        // 상대방 플레이어가 선택할 수 없도록 하면서도, 씬 전환
        if (!info.Sender.IsLocal)
        {
            // 상대방에게 씬 전환을 알림
            StartCoroutine(LoadCardSelectAfterDelay(3f)); // 상대방도 이동
        }
    }



    private IEnumerator LoadCardSelectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // CardSelect 씬으로 이동
        PhotonNetwork.LoadLevel("01_Scenes/03CardGameVR/CardSelect");
    }
}
