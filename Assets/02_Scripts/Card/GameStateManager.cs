using UnityEngine;
using Photon.Pun;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public string WinCondition { get; private set; } // 승리 조건

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetWinCondition(string condition)
    {
        WinCondition = condition;

        // 승리 조건을 Photon Custom Properties에 저장
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
        {
            { "winCondition", condition }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        Debug.Log($"Win condition set: {condition}"); // 로그 추가
    }


}
