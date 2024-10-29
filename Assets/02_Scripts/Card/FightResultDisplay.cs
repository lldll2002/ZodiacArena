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
    [Header("UI")]
    [SerializeField] private TMP_Text resultText; // 승리 결과 표시 텍스트
    private int winCount; // 현재 승리 횟수 저장


    [Header("Effect")]
    [SerializeField] private GameObject winnerEffect; // 승자 이펙트를 인스펙터에 연결할 변수 
    [SerializeField] private GameObject loserEffect; // 패자 이펙트를 인스펙터에 연결할 변수 
    [SerializeField] private Transform EffectSpawnPoint;

    [Header("Model")]
    [SerializeField] private Transform modelSpawnPoint; // 모델이 생성될 위치
    [SerializeField] private GameObject[] zodiacPrefabs; // 모델 프리팹 배열

    [Header("SFX")]
    [SerializeField] private AudioClip winSFX; // 승리 SFX
    [SerializeField] private AudioClip loseSFX; // 패배 SFX
    private AudioSource audioSource; // AudioSource 컴포넌트

    async void Start()
    {

        // Unity 서비스 초기화
        await UnityServices.InitializeAsync();

        // AudioSource 컴포넌트 생성 및 설정
        audioSource = gameObject.AddComponent<AudioSource>();


        // 플레이어가 선택한 카드 번호 가져오기
        int selectedCard = PlayerPrefs.GetInt("SelectedCard", -1);

        // 유효한 카드 번호일 때만 모델 생성
        if (selectedCard >= 1 && selectedCard <= zodiacPrefabs.Length)
        {
            Instantiate(zodiacPrefabs[selectedCard - 1], modelSpawnPoint.position, Quaternion.identity);
        }

        // PlayerPrefs에서 플레이어 승리 여부를 가져옴
        int playerWon = PlayerPrefs.GetInt("PlayerWon", -1); // 기본값 -1로 설정
        Debug.Log($"{playerWon}");


        #region 승리/패배 효과 + 모델 구현
        // 플레이어가 이겼을 경우 메시지 표시 및 승리 횟수 업데이트
        if (playerWon == 1)
        {
            resultText.text = "You win!";
            Instantiate(winnerEffect, EffectSpawnPoint);
            PlaySFX(winSFX);
            await UpdateWinCount(); // 승리 횟수 업데이트
        }
        else if (playerWon == 0)
        {
            resultText.text = "You lost!";
            Instantiate(loserEffect, EffectSpawnPoint);
            PlaySFX(loseSFX);
        }
        else
        {
            resultText.text = "It's a tie!";
        }

        // 승패에 따른 이펙트 실행
        //ShowEffect();

        #endregion

        // 플레이어 닉네임 불러오기
        await LoadPlayerNickName();

        // 3초 후에 Ranking 씬으로 넘어가기
        Invoke("LoadRankingScene", 3f);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip); // 지정한 클립을 한 번 재생
        }
        else
        {
            Debug.LogWarning("SFX 클립이 없습니다.");
        }
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
