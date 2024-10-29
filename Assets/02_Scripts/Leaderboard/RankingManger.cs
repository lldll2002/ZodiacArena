using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Unity.Services.Leaderboards;
using Unity.Services.CloudSave;
using Unity.Services.Leaderboards.Models;

using UnityEngine.UI;
using UnityEngine.SceneManagement; // 씬 관리용 네임스페이스 추가

public class RankingManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text[] rankTexts; // 랭킹 표시용 TMP 텍스트 배열
    [SerializeField] private Button returnToLobbyButton; // 로비로 돌아가는 버튼

    private const string leaderboardId = "Ranking"; // 리더보드 ID
    public List<LeaderboardEntry> entries = new(); // 점수 저장 리스트

    private async void Awake()
    {
        // 로그인 되어 있다고 가정하고 바로 랭킹 데이터 불러오기
        await LoadAndDisplayRankings();

        // 버튼 클릭 이벤트에 로비 씬으로 이동하는 함수 연결
        returnToLobbyButton.onClick.AddListener(ReturnToLobby);
    }

    private async Task LoadAndDisplayRankings()
    {
        try
        {
            var response = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId);
            entries = response.Results;

            Dictionary<string, string> playerNames = new();

            foreach (var entry in entries)
            {
                var playerData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { entry.PlayerId });

                if (playerData != null && playerData.Count > 0)
                {
                    string playerName = playerData.ContainsKey("playerName") ? playerData["playerName"].ToString() : "Unknown Player";
                    playerNames[entry.PlayerId] = playerName;
                }
                else
                {
                    playerNames[entry.PlayerId] = "Unknown Player";
                }
            }

            var sortedEntries = entries.OrderByDescending(entry => entry.Score).ToList();

            for (int i = 0; i < rankTexts.Length; i++)
            {
                if (i < sortedEntries.Count)
                {
                    var entry = sortedEntries[i];
                    string playerName = playerNames[entry.PlayerId];
                    rankTexts[i].text = $"{playerName}: {entry.Score}";
                }
                else
                {
                    rankTexts[i].text = "";
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"랭킹 데이터 로드 실패: {e.Message}");
        }
    }

    // 로비 씬으로 이동하는 함수
    private void ReturnToLobby()
    {
        SceneManager.LoadScene("01_Scenes/02CardGameVR/Lobby");
    }
}
