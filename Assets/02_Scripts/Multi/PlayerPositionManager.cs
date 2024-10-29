using System.Collections;
using Photon.Pun;
using UnityEngine;

public class PlayerPositionManager : MonoBehaviourPunCallbacks
{
    // 싱글톤 만들기
    public static PlayerPositionManager Instance = null;

    [SerializeField] private Transform spawnPoint1; // PlayerRole이 0인 플레이어 스폰 포인트
    [SerializeField] private Transform spawnPoint2; // PlayerRole이 1인 플레이어 스폰 포인트
    [SerializeField] private GameObject player1Canvas; // Player1용 Canvas
    [SerializeField] private GameObject player2Canvas; // Player2용 Canvas

    private PhotonView pv;

    void Awake()
    {
        Instance = this;
    }

    IEnumerator Start()
    {
        pv = GetComponent<PhotonView>();

        yield return new WaitForSeconds(0.5f);
        CreatePlayer();
    }

    void CreatePlayer()
    {
        // 저장된 PlayerRole 가져오기
        int playerRole = PlayerPrefs.GetInt("PlayerRole", 0);

        Debug.Log($"CoinFlip Scene Player Position : {playerRole}");

        // 스폰 포인트 선택
        Transform selectedSpawnPoint = (playerRole == 0) ? spawnPoint1 : spawnPoint2;

        // 선택된 스폰 포인트의 위치와 회전 사용
        Vector3 pos = selectedSpawnPoint.position;
        Quaternion rot = selectedSpawnPoint.rotation;

        // 플레이어 생성
        GameObject player = PhotonNetwork.Instantiate("Player", pos, rot, 0);

        // 역할에 맞는 Canvas 활성화
        ActivateRoleSpecificCanvas(playerRole);
    }

    private void ActivateRoleSpecificCanvas(int playerRole)
    {
        // Canvas 활성화 설정
        if (playerRole == 0)
        {
            if (player1Canvas != null) player1Canvas.SetActive(true);
            if (player2Canvas != null) player2Canvas.SetActive(false);
        }
        else if (playerRole == 1)
        {
            if (player1Canvas != null) player1Canvas.SetActive(false);
            if (player2Canvas != null) player2Canvas.SetActive(true);
        }
    }
}
