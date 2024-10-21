using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.CloudSave; // Cloud Save 사용을 위해 추가
using System.Threading.Tasks;

public class FightResultManager : MonoBehaviour
{
    [SerializeField] private TMP_Text resultText; // 승리 결과 표시 텍스트
    private int winCount; // 현재 승리 횟수 저장

    async void Start()
    {
        // PlayerPrefs에서 플레이어 승리 여부를 가져옴
        int playerWon = PlayerPrefs.GetInt("PlayerWon", -1); // 기본값 -1로 설정

        // 플레이어가 이겼을 경우 메시지 표시 및 승리 횟수 업데이트
        if (playerWon == 1)
        {
            resultText.text = "You win!";
            await UpdateWinCount(); // 승리 횟수 업데이트
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

    // 클라우드에 winCount 업데이트 함수
    private async Task UpdateWinCount()
    {
        try
        {
            // 클라우드에서 기존 winCount 불러오기
            var savedData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "winCount" });

            if (savedData.ContainsKey("winCount"))
            {
                winCount = int.Parse(savedData["winCount"].ToString());
            }
            else
            {
                winCount = 0; // winCount가 없을 경우 0으로 초기화
            }

            // 승리 횟수 증가
            winCount++;

            // 클라우드에 승리 횟수 저장
            var data = new Dictionary<string, object>
            {
                { "winCount", winCount }
            };

            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
            Debug.Log($"승리 횟수 업데이트: {winCount}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Cloud Save 업데이트 실패: {e.Message}");
        }
    }
}
