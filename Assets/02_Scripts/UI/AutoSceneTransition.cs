using System.Collections;
using UnityEngine;

public class AutoSceneTransition : MonoBehaviour
{
    public SceneTransitionManager sceneTransitionManager; // 씬 전환 매니저
    public float waitBeforeTransition = 2f; // 자동 전환 전 대기 시간
    public int targetSceneIndex = 1; // 전환할 씬의 인덱스

    void Start()
    {
        StartCoroutine(AutoTransitionRoutine());
    }

    IEnumerator AutoTransitionRoutine()
    {
        // 대기 시간 후 씬 전환 시작
        yield return new WaitForSeconds(waitBeforeTransition);

        // SceneTransitionManager에서 씬 전환 실행
        sceneTransitionManager.GoToScene(targetSceneIndex);
    }
}