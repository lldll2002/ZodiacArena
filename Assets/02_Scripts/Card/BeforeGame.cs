using Photon.Pun;
using TMPro; // TMP_Text 사용을 위해 추가
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections.Generic;

public class BeforeGame : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    [SerializeField] private TMP_Text resultText; // 동전 던지기 결과 텍스트
    [SerializeField] private TMP_Text selectionText; // 선택 중 플레이어 이름을 표시할 텍스트
    [SerializeField] private TMP_Text playerNamesText; // 플레이어 이름 표시할 텍스트

    [Header("Select High/Low")]
    [SerializeField] private Button highButton; // 하이 선택 버튼
    [SerializeField] private Button lowButton; // 로우 선택 버튼

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
        // 방에 두 명의 플레이어가 있는지 확인
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            Debug.Log("Waiting for another player to join...");
            return; // 플레이어가 부족할 경우 조기 반환
        }



        // if (PhotonNetwork.IsMasterClient)
        // {
        //     Hashtable ht = new Hashtable() { { "HIGH_LOW", Random.Range(0, 2) } };
        //     photonView.Owner.SetCustomProperties(ht);
        // }


        // 랜덤하게 Heads 또는 Tails 결정
        bool isHeads = Random.Range(0, 2) == 0; // 0: Heads, 1: Tails
        resultText.text = $"Coin flipped: {(isHeads ? "Heads" : "Tails")}"; // TMP_Text에 결과 표시

        /*
                        // 역할을 할당
                        //int headPlayerActorNumber = isHeads ? PhotonNetwork.LocalPlayer.ActorNumber : GetOtherPlayer().ActorNumber;
                        //int tailsPlayerActorNumber = isHeads ? GetOtherPlayer().ActorNumber : PhotonNetwork.LocalPlayer.ActorNumber;

                        int firstNumber = Random.Range(0, 2);   //0,1
                        int secondNumber = Random.Range(0, 2);  //0,1

                        while (firstNumber == secondNumber)
                        {
                            secondNumber = Random.Range(0, 2);
                        }

                        int headPlayerActorNumber = firstNumber;
                        int tailsPlayerActorNumber = secondNumber;
                        */


        // 플레이어 할당
        int headPlayerActorNumber = PhotonNetwork.PlayerList[0].ActorNumber;
        int tailsPlayerActorNumber = PhotonNetwork.PlayerList[1].ActorNumber;


        Debug.Log($"Head Player: {headPlayerActorNumber}, Tails Player: {tailsPlayerActorNumber}");

        // RPC 호출을 통해 각 플레이어가 자신의 역할을 알 수 있게 함
        photonView.RPC("AssignRolesRPC", RpcTarget.All, headPlayerActorNumber, tailsPlayerActorNumber, isHeads);
    }

    // 모든 클라이언트에서 호출될 RPC 함수
    [PunRPC]
    private void AssignRolesRPC(int headPlayerActorNumber, int tailsPlayerActorNumber, bool isHeads)
    {
        Photon.Realtime.Player headPlayer = PhotonNetwork.CurrentRoom.GetPlayer(headPlayerActorNumber);
        Photon.Realtime.Player tailsPlayer = PhotonNetwork.CurrentRoom.GetPlayer(tailsPlayerActorNumber);

        // 플레이어가 null인지 확인
        if (headPlayer == null || tailsPlayer == null)
        {
            Debug.LogError("Player not found! Check the ActorNumber.");
            return; // 플레이어가 없으면 조기 반환
        }

        // 각 플레이어에 따라 턴을 할당
        if (PhotonNetwork.LocalPlayer.ActorNumber == headPlayerActorNumber)
        {
            AssignTurn(PhotonNetwork.LocalPlayer, true); // Head 플레이어에게 선택 버튼 활성화
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == tailsPlayerActorNumber)
        {
            AssignTurn(PhotonNetwork.LocalPlayer, false); // Tails 플레이어에게 선택 버튼 비활성화
        }
        else
        {
            Debug.LogWarning("Unexpected player assignment during role assignment.");
        }

        // Debug Log 추가: 역할 할당 정보 확인
        Debug.Log($"Role assignment: {headPlayer.NickName} (Heads), {tailsPlayer.NickName} (Tails)");
    }

    private Photon.Realtime.Player GetOtherPlayer()
    {
        return PhotonNetwork.PlayerList[0].NickName == PhotonNetwork.LocalPlayer.NickName
            ? PhotonNetwork.PlayerList[1]
            : PhotonNetwork.PlayerList[0];
    }

    private void AssignTurn(Photon.Realtime.Player player, bool canSelect)
    {
        Debug.Log($"{player.NickName} is assigned as {(canSelect ? "Heads" : "Tails")}.");

        if (canSelect) // 하이/로우 선택 가능 여부에 따라
        {
            highButton.gameObject.SetActive(true);
            lowButton.gameObject.SetActive(true);
            selectionText.text = "You are selecting High/Low"; // 선공 플레이어에게 메시지 표시
        }
        else
        {
            highButton.gameObject.SetActive(false); // 선택할 수 없도록 비활성화
            lowButton.gameObject.SetActive(false);
            selectionText.text = $"{player.NickName} is waiting..."; // 후공 플레이어는 기다림
        }
    }

    private void OnWinConditionSelected(string condition)
    {
        // 승리 조건을 다른 플레이어에게 전파 (RPC로만 전달하는 대신 CustomProperties 사용)
        Hashtable ht = new Hashtable { { "WinCondition", condition } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);

        // 버튼 비활성화 및 UI 업데이트
        highButton.gameObject.SetActive(false);
        lowButton.gameObject.SetActive(false);
        selectionText.text = $"Selected win condition: {condition}";

        // 모든 플레이어가 선택했는지를 확인
        StartCoroutine(LoadCardSelectAfterDelay(3f)); // 3초 대기 후 CardSelect 씬으로 이동
    }

    // 모든 플레이어가 WinCondition을 확인할 수 있도록 설정
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("WinCondition"))
        {
            string winCondition = (string)PhotonNetwork.CurrentRoom.CustomProperties["WinCondition"];
            selectionText.text = $"Selected win condition: {winCondition}";
            Debug.Log($"All players received win condition: {winCondition}");

            // 모든 플레이어가 같은 WinCondition으로 게임을 진행하도록 설정
            PlayerPrefs.SetString("WinCondition", winCondition);
            PlayerPrefs.Save();
        }
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
        PhotonNetwork.LoadLevel("01_Scenes/02CardGameVR/CardSelect");
    }
}