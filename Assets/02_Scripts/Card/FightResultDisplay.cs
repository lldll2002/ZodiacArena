using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager를 사용하기 위해 추가

public class FightResultManager : MonoBehaviour
{
    [SerializeField] private TMP_Text resultText; // 승리 결과 표시 텍스트

    void Start()
    {
        // PlayerPrefs에서 플레이어 승리 여부를 가져옴
        int playerWon = PlayerPrefs.GetInt("PlayerWon", -1); // 기본값 -1로 설정

        // 플레이어가 이겼을 경우 메시지 표시
        if (playerWon == 1)
        {
            resultText.text = "You win!";
        }
        else if (playerWon == 0)
        {
            resultText.text = "You lost!";
        }
        else
        {
            resultText.text = "It's a tie!";
        }

        // 3초 후에 Ranking 씬으로 넘어가기
        Invoke("LoadRankingScene", 3f);
    }

    private void LoadRankingScene()
    {
        SceneManager.LoadScene("Ranking"); // Ranking 씬으로 전환
    }
}
