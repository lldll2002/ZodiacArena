using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 씬 전환을 위한 추가

public class ChangeName : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text Info;

    [Header("Buttons")]
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    void Start()
    {
        // PlayerPrefs에서 저장된 닉네임 가져오기
        string existingNickName = PlayerPrefs.GetString("NICK_NAME", "");

        // 저장된 닉네임이 없으면 InputName 씬으로 이동
        if (string.IsNullOrEmpty(existingNickName))
        {
            SceneManager.LoadScene("01_Scenes/01Title/InputName");
        }
        else
        {
            // 저장된 닉네임을 Nick_Name 변수에 저장
            PlayerPrefs.SetString("Nick_Name", existingNickName);

            // 메시지 설정
            Info.text = $"Do you want to change your name from {existingNickName}?";

            // 버튼 이벤트 연결
            yesButton.onClick.AddListener(OnYesButtonClick);
            noButton.onClick.AddListener(OnNoButtonClick);
        }
    }

    void OnYesButtonClick()
    {
        // InputName 씬으로 이동
        SceneManager.LoadScene("01_Scenes/01Title/InputName");
    }

    void OnNoButtonClick()
    {
        // 익명 로그인 처리 및 Lobby 씬으로 이동
        // 여기서 익명 로그인 처리 로직을 추가
        // ...

        SceneManager.LoadScene("Lobby"); // "Lobby" 씬의 실제 이름으로 변경
    }
}
