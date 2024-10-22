using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.CloudSave;
using Unity.Services.Leaderboards; // LeaderboardsService 사용을 위해 추가
using System.Threading.Tasks;
using Unity.Services.Core;

public class FightResultManager : MonoBehaviour
{
    [SerializeField] private TMP_Text resultText; // 승리 결과 표시 텍스트
    private int winCount; // 현재 승리 횟수 저장

    async void Start()
    {
        // Unity 서비스 초기화
        await UnityServices.InitializeAsync();

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

        // 플레이어 닉네임 불러오기
        await LoadPlayerNickName();

        // 3초 후에 Ranking 씬으로 넘어가기
        Invoke("LoadRankingScene", 3f);
    }

    private async Task LoadPlayerNickName()
    {
        try
        {
            // 클라우드에서 플레이어 닉네임 불러오기
            var savedData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "playerName" });

            // savedData가 null인지 확인
            if (savedData == null)
            {
                Debug.LogError("savedData가 null입니다.");
                return;
            }

            if (savedData.ContainsKey("playerName"))
            {
                string playerName = savedData["playerName"].ToString();
                Debug.Log($"플레이어 닉네임: {playerName}");
            }
            else
            {
                Debug.LogWarning("플레이어 닉네임이 Cloud Save에 존재하지 않습니다.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"닉네임 불러오기 실패: {e.Message}");
        }
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

            // LeaderboardsService에 승리 횟수 제출
            await SubmitScoreToLeaderboard(winCount);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Cloud Save 업데이트 실패: {e.Message}");
        }
    }

    // LeaderboardsService에 점수 제출
    private async Task SubmitScoreToLeaderboard(int score)
    {
        try
        {
            await LeaderboardsService.Instance.AddPlayerScoreAsync("Ranking", score);
            Debug.Log($"리더보드에 점수 제출: {score}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"리더보드에 점수 제출 실패: {e.Message}");
        }
    }

    // 플레이어 닉네임 업데이트 함수
    private async Task UpdatePlayerNickName(string newNickName)
    {
        try
        {
            var data = new Dictionary<string, object>
            {
                { "playerName", newNickName }
            };

            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
            Debug.Log($"플레이어 닉네임 '{newNickName}'이(가) Cloud Save에 업데이트되었습니다.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"닉네임 업데이트 실패: {e.Message}");
        }
    }
}
