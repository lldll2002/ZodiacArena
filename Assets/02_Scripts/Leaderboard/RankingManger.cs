using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Unity.Services.Leaderboards;
using Unity.Services.CloudSave;
using Unity.Services.Leaderboards.Models;

public class RankingManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text[] rankTexts; // 랭킹 표시용 TMP 텍스트 배열

    private const string leaderboardId = "Ranking"; // 리더보드 ID
    public List<LeaderboardEntry> entries = new(); // 점수 저장 리스트

    private async void Awake()
    {
        // 로그인이 이미 되어 있다고 가정하고 바로 랭킹 데이터를 불러오기
        await LoadAndDisplayRankings();
    }

    private async Task LoadAndDisplayRankings()
    {
        try
        {
            // 리더보드에서 모든 플레이어 점수 가져오기
            var response = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId);
            entries = response.Results;

            // 모든 플레이어의 점수 및 이름을 가져오기 위해 Dictionary를 사용
            Dictionary<string, string> playerNames = new();

            // Cloud Save에서 모든 플레이어의 이름을 불러오기
            foreach (var entry in entries)
            {
                Debug.Log($"Loading player data for PlayerId: {entry.PlayerId}"); // PlayerId 로그
                var playerData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { entry.PlayerId });

                // 클라우드 데이터가 있는지 확인
                if (playerData != null && playerData.Count > 0)
                {
                    foreach (var data in playerData)
                    {
                        Debug.Log($"Key: {data.Key}, Value: {data.Value}"); // 각 데이터 로그
                    }

                    // 각 플레이어의 이름을 Cloud Save에서 가져오기
                    string playerName = playerData.ContainsKey("playerName") ? playerData["playerName"].ToString() : "Unknown Player";
                    playerNames[entry.PlayerId] = playerName;
                }
                else
                {
                    Debug.Log($"No data found for PlayerId: {entry.PlayerId}");
                    playerNames[entry.PlayerId] = "Unknown Player"; // 데이터가 없을 경우 기본값 설정
                }
            }

            // 상위 플레이어를 내림차순으로 정렬
            var sortedEntries = entries.OrderByDescending(entry => entry.Score).ToList();

            // 최대 5명의 플레이어를 TMP 텍스트에 표시
            for (int i = 0; i < rankTexts.Length; i++)
            {
                if (i < sortedEntries.Count)
                {
                    var entry = sortedEntries[i];
                    string playerName = playerNames[entry.PlayerId]; // 매칭된 플레이어 이름
                    rankTexts[i].text = $"{playerName}: {entry.Score}";
                }
                else
                {
                    rankTexts[i].text = ""; // 남은 텍스트는 공백으로 설정
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"랭킹 데이터 로드 실패: {e.Message}");
        }
    }

}
