using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine;
using Unity.Services.Core;


[System.Serializable]
public struct PlayerData
{
    public string name;
    public int score;
}

public class LoginManager : MonoBehaviour
{
    [Header("Player Data")]
    public PlayerData playerData;

    private async void Awake()
    {
        // UGS 초기화 (AuthenticationService와 CloudSaveService를 사용할 수 있습니다.)
        await InitializeServices();

        // 익명 로그인 자동 호출
        await SignIn();
    }

    private async Task InitializeServices()
    {
        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services 초기화 성공");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Unity Services 초기화 실패: {ex.Message}");
        }
    }

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

    #region Cloud Save
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
