using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.CloudSave;
using Unity.Services.Leaderboards; // LeaderboardsService 사용을 위해 추가
using System.Threading.Tasks;
using Unity.Services.Core;

public class TestEffect : MonoBehaviour
{
    [Header("Test Number")]
    [SerializeField] private int playerWon;


    [Header("Effect Prefab")]
    [SerializeField] private GameObject winnerEffect; // 승자 이펙트를 인스펙터에 연결할 변수 
    [SerializeField] private GameObject loserEffect; // 패자 이펙트를 인스펙터에 연결할 변수 

    [Header("SpawnPoint")]
    [SerializeField] private Transform EffectSpawnPoint;

    async void Start()
    {
        #region 승리/패배 효과 + 모델 구현
        // 플레이어가 이겼을 경우 메시지 표시 및 승리 횟수 업데이트
        if (playerWon == 1)
        {
            Instantiate(winnerEffect, EffectSpawnPoint);
        }
        else if (playerWon == 0)
        {
            Instantiate(loserEffect, EffectSpawnPoint);
        }
        else
        {
        }

        #endregion


        // 자동으로 3초 후에 Ranking 씬으로 넘어가기
        //Invoke("LoadRankingScene", 3f);
    }


    private void LoadRankingScene()
    {
        SceneManager.LoadScene("Ranking"); // Ranking 씬으로 전환
    }


}
