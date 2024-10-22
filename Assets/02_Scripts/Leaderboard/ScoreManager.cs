using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button allScoreButton;

    private const string leaderboardId = "Ranking";

    // 점수를 저장할 리스트
    public List<LeaderboardEntry> entries = new();

    //===========================================================
    private async void Awake()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += async () =>
        {
            Debug.Log("로그인 완료");

            //기존점수 불러오기
            await GetPlayerScore();
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        // 모든 Score 버튼 클릭 이벤트 연결
        allScoreButton.onClick.AddListener(async () => await GetAllScore());
    }

    //======================================================
    private async Task AddScore(int score)
    {
        var response = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, score);

        //Json 파일 형식을 읽어서 형태 변환 --> 출력.
        Debug.Log(JsonConvert.SerializeObject(response));
    }

    //======================================================
    private async Task GetPlayerScore()
    {
        var response = await LeaderboardsService.Instance.GetPlayerScoreAsync(leaderboardId);


        Debug.Log(JsonConvert.SerializeObject(response));
    }

    //======================================================
    private async Task GetAllScore()
    {
        var response = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId);

        Debug.Log(JsonConvert.SerializeObject(response));

        entries = response.Results;

        string ranking = "";
        foreach (var entry in entries)
        {
            ranking += $"[{entry.Rank + 1}] {entry.PlayerName} : {entry.Score}\n";
        }

        Debug.Log(ranking);

        //milliseconds 기준이라, 2초 기다렸다가
        //들고 오도록 처리
        await Task.Delay(2000);
        await GetScoreByPage();
    }

    //======================================================
    // 페이징 처리
    private async Task GetScoreByPage()
    {
        //플레이어 수가 많으면, 한 페이지에 보일 랭킹의 수를 조절.
        var options = new GetScoresOptions
        {
            //1등부터 20등까지 들고 옴.
            Offset = 1,
            Limit = 20
        };

        // 높은 등수 5 + 자기 자신 등수 + 낮은 등수 5
        var options2 = new GetPlayerRangeOptions
        {
            RangeLimit = 5
        };

        var response = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId, options);
        Debug.Log(JsonConvert.SerializeObject(response));
    }

}
