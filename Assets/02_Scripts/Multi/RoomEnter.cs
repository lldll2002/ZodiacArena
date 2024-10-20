using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class RoomEnter : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text playerNumText;
    [SerializeField] private TMP_Text playerEnterText;
    [SerializeField] private TMP_Text infoText;

    private void Start()
    {
        DisplayPlayerNickName();
        DisplayConnectInfo();
    }

    //================================================

    public void DisplayConnectInfo()
    {
        UpdatePlayerNumText();
        UpdateInfoText(PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);
    }

    private void UpdatePlayerNumText()
    {
        int currPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
        int maxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers;
        playerNumText.text = $"({currPlayer}/{maxPlayer})";
    }

    private void UpdateInfoText(int currPlayer, int maxPlayer)
    {
        if (currPlayer < maxPlayer)
        {
            infoText.text = "Please wait...";
        }
        else if (currPlayer == maxPlayer)
        {
            infoText.text = "Starting the game...";
            Invoke(nameof(LoadCoinFlipScene), 3f); // 3초 후에 LoadCoinFlipScene 메서드 호출
        }
    }

    public void DisplayPlayerNickName()
    {
        string playerNickName = PhotonNetwork.NickName;
        playerEnterText.text = playerNickName + " entered the room.";
    }

    //================================================

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("01_Scenes/03CardGameVR/Lobby");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        DisplayConnectInfo(); // 플레이어가 들어오면 정보를 업데이트
        DisplayPlayerNickName(); // 플레이어 닉네임 표시

        // 방에 들어온 플레이어의 닉네임을 다른 플레이어에게도 알림
        playerEnterText.text = newPlayer.NickName + " entered the room.";
    }

    private void LoadCoinFlipScene()
    {
        SceneManager.LoadScene("01_Scenes/03CardGameVR/CoinFlip"); // CoinFlip 씬으로 전환
    }
}
