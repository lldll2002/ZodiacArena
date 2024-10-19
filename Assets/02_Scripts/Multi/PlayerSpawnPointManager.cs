using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // Photon 네트워크 관련 네임스페이스

public class PlayerSpawnPointManager : MonoBehaviourPunCallbacks
{
    public static PlayerSpawnPointManager Instance = null;

    public List<Transform> spawnPoints = new List<Transform>(); // 스폰 포인트 목록
    public GameObject playerPrefab; // 플레이어 프리팹

    private bool isGameOver = false;

    public bool IsGameOver
    {
        get { return isGameOver; }
        set { isGameOver = value; }
    }

    #region Awake
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    #region Start
    void Start()
    {
        // 스폰 포인트 그룹을 찾고 자식들을 리스트에 추가
        var spawnPointGroup = GameObject.Find("PlayerSpawnPointGroup");
        spawnPointGroup.GetComponentsInChildren<Transform>(spawnPoints);

        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            // 마스터 클라이언트가 각 플레이어에게 스폰 포인트를 할당합니다.
            AssignSpawnPoints();
        }
    }
    #endregion

    #region AssignSpawnPoints
    private void AssignSpawnPoints()
    {
        // Photon Network의 모든 플레이어에게 스폰 포인트를 할당
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (i < spawnPoints.Count)
            {
                // 각 플레이어에게 고유한 스폰 포인트를 할당
                Photon.Realtime.Player player = PhotonNetwork.PlayerList[i];
                photonView.RPC("SpawnPlayerAtPoint", player, spawnPoints[i].position, spawnPoints[i].rotation);
            }
        }
    }
    #endregion

    #region SpawnPlayerAtPoint (RPC)
    [PunRPC]
    void SpawnPlayerAtPoint(Vector3 position, Quaternion rotation)
    {
        // 플레이어를 지정된 위치와 회전값에 스폰
        PhotonNetwork.Instantiate(playerPrefab.name, position, rotation);
    }
    #endregion
}
