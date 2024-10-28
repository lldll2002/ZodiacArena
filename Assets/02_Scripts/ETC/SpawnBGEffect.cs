using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBGEffect : MonoBehaviour
{
    public GameObject prefabToSpawn; // 생성할 프리팹
    public Transform[] spawnPoints;  // 스폰 포인트 배열

    void Start()
    {
        // 모든 스폰 포인트에 프리팹 생성
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpawnPrefabAtPoint(i);
        }
    }

    // 지정된 위치에 프리팹을 생성하는 함수
    public void SpawnPrefabAtPoint(int pointIndex)
    {
        if (pointIndex < 0 || pointIndex >= spawnPoints.Length)
        {
            Debug.LogWarning("잘못된 인덱스입니다. 스폰 포인트가 존재하지 않습니다. pointIndex: " + pointIndex);
            return;
        }

        Transform spawnPoint = spawnPoints[pointIndex];
        Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("프리팹 생성 완료, 위치: " + spawnPoint.position);
    }
}
