using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LobbyLoginManager : MonoBehaviour
{
    private LoginManager loginManager;

    void Start()
    {
        // LoginManager를 찾아서 저장
        loginManager = FindObjectOfType<LoginManager>();

        // PlayerPrefs에서 저장된 닉네임 가져오기
        if (PlayerPrefs.HasKey("NICK_NAME"))
        {
            string nickName = PlayerPrefs.GetString("NICK_NAME");
            // 닉네임을 Cloud Save에 업데이트
            loginManager?.UpdatePlayerNickName(nickName);
        }
    }
}
