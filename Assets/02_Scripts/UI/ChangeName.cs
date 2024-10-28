using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 씬 전환을 위한 추가
using System.Collections; // 코루틴을 위한 추가

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
            yesButton.onClick.AddListener(() => StartCoroutine(OnYesButtonClick()));
            noButton.onClick.AddListener(() => StartCoroutine(OnNoButtonClick()));
        }
    }

    IEnumerator OnYesButtonClick()
    {
        // 1초 동안 대기
        yield return new WaitForSeconds(1f);

        // InputName 씬으로 이동
        SceneManager.LoadScene("01_Scenes/01Title/InputName");
    }

    IEnumerator OnNoButtonClick()
    {
        // 1초 동안 대기
        yield return new WaitForSeconds(1f);

        // Lobby 씬으로 이동
        SceneManager.LoadScene("01_Scenes/02CardGameVR/Lobby"); // "Lobby" 씬의 실제 이름으로 변경
    }
}
