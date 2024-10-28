using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InputNameManager : MonoBehaviour
{
    #region Settings
    [Header("UI")]
    [SerializeField] private TMP_InputField nickNameIf;

    [Header("Button")]
    [SerializeField] private Button enterButton; // 확인 버튼
    #endregion

    private string nickName;

    //---------------------------------------------------------
    #region Awake & Start

    void Start()
    {
        // PlayerPrefs에서 저장된 닉네임이 있는지 확인
        if (PlayerPrefs.HasKey("NICK_NAME"))
        {
            nickName = PlayerPrefs.GetString("NICK_NAME");
            // 닉네임을 InputField에 설정하지 않음
        }

        // 버튼 이벤트 연결
        enterButton.onClick.AddListener(OnEnterButtonClick);
    }
    #endregion

    //---------------------------------------------------------
    #region Nickname Handling
    public void OnEnterButtonClick()
    {
        // 닉네임이 비어있지 않은지 확인
        if (!string.IsNullOrEmpty(nickNameIf.text))
        {
            nickName = nickNameIf.text;
            PlayerPrefs.SetString("NICK_NAME", nickName); // 입력된 닉네임을 PlayerPrefs에 저장
        }

        // 이후 로비 씬으로 전환
        SceneManager.LoadScene("01_Scenes/01Title/InputBirthday"); // 다음 씬으로 전환
    }
    #endregion
}
