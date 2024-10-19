using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;


public class GameManager : MonoBehaviourPunCallbacks
{
    // 싱글톤 만들기
    public static GameManager Instance = null;

    [SerializeField] private Button exitButton;
    [SerializeField] private Transform[] spawnPoints; // 스폰 포인트 배열

    private PhotonView pv;

    void Awake()
    {
        Instance = this;
    }

    IEnumerator Start()
    {
        exitButton.onClick.AddListener(() => OnExitButtonClick());

        pv = GetComponent<PhotonView>();

        yield return new WaitForSeconds(0.5f);
        CreatePlayer();
    }

    void CreatePlayer()
    {
        // 스폰 포인트 중에서 랜덤으로 선택
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 선택된 스폰 포인트의 위치와 회전 사용
        Vector3 pos = spawnPoint.position;
        Quaternion rot = spawnPoint.rotation;

        // 플레이어 생성
        GameObject player = PhotonNetwork.Instantiate("Player", pos, rot, 0);

    }

    #region 사용자 정의 콜백
    private void OnExitButtonClick()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion

    #region 포톤 콜백함수
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("01_Scenes/Lobby");
    }
    #endregion
}
