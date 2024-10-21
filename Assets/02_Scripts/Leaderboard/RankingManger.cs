using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;

public class RankingManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text[] rankTexts; // 랭킹 표시용 TMP 텍스트 배열

    private const string leaderboardId = "Ranking"; // 리더보드 ID
    public List<LeaderboardEntry> entries = new(); // 점수 저장 리스트

    private async void Awake()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += async () =>
        {
            Debug.Log("로그인 완료");
            // 랭킹 데이터 불러오기
            await LoadAndDisplayRankings();
        };

        // 익명으로 로그인
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private async Task LoadAndDisplayRankings()
    {
        try
        {
            // 모든 플레이어 점수 가져오기
            var response = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId);

            //Debug.Log(JsonConvert.SerializeObject(response));
            entries = response.Results;

            // 승리 횟수에 따라 정렬 (내림차순)
            var topPlayers = entries.OrderByDescending(entry => entry.Score).Take(5).ToList();

            // 상위 5명의 점수와 이름을 TMP 텍스트에 표시
            for (int i = 0; i < topPlayers.Count; i++)
            {
                rankTexts[i].text = $"{topPlayers[i].PlayerName}: {topPlayers[i].Score}"; // 플레이어 이름과 점수 출력
            }

            // 5명 미만일 경우 남은 텍스트는 공백으로 설정
            for (int i = topPlayers.Count; i < rankTexts.Length; i++)
            {
                rankTexts[i].text = "";
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"랭킹 데이터 로드 실패: {e.Message}");
        }
    }
}
