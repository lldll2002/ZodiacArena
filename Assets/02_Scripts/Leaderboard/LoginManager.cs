using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;

[System.Serializable]
public struct PlayerData
{
    public string name;
    public int winCount;
}

public class LoginManager : MonoBehaviour
{
    [Header("Player Data")]
    public PlayerData playerData;

    // 1. UGS 초기화
    private async void Awake()
    {
        // 1-1. UGS 초기화 성공 시 호출되는 콜백
        UnityServices.Initialized += () =>
        {
            Debug.Log("UGS 초기화 성공");
        };

        // 1-2. UGS 초기화 실패 시 호출되는 콜백
        UnityServices.InitializeFailed += (ex) =>
        {
            Debug.Log($"UGS 초기화 실패: {ex.Message}");
        };

        // 1-3. Unity 초기화
        await UnityServices.InitializeAsync();

        // 1-4. 익명 로그인 자동 호출
        await SignIn();
    }

    //=====================================================
    #region Login
    private async Task SignIn()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            string playerId = AuthenticationService.Instance.PlayerId;
            Debug.Log($"익명 로그인 성공:\nPlayerId: <color=#00ff00>{playerId}</color>");
        }
        catch (AuthenticationException e)
        {
            Debug.LogError($"익명 로그인 실패: {e.Message}");
        }
    }
    #endregion

    //=====================================================
    #region Cloud Save
    // 플레이어 닉네임 업데이트 함수
    public async void UpdatePlayerNickName(string newNickName)
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
        catch (Exception e)
        {
            Debug.LogError($"Cloud Save 업데이트 실패: {e.Message}");
        }
    }
    #endregion
}
