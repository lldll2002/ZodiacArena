using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 추가된 부분

public class InputNameManager : MonoBehaviourPunCallbacks
{
    #region Settings
    [Header("UI")]
    [SerializeField] private TMP_InputField nickNameIf;

    [Header("Button")]
    [SerializeField] private Button enterLobbyButton;
    #endregion

    private string nickName; // 여기에 닉네임 변수를 추가합니다.
    private LoginManager loginManager;

    //---------------------------------------------------------
    #region Awake & Start

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
        enterLobbyButton.onClick.AddListener(() => OnLoginButtonClick());
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

        // 로비 씬으로 전환
        SceneManager.LoadScene("01_Scenes/03CardGameVR/Lobby"); // "LobbyScene"을 실제 로비 씬의 이름으로 변경하세요.
    }
    #endregion
}
