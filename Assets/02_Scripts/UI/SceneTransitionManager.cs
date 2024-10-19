using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public FadeScreen fadeScreen;
    public AudioSource audioSource; // 씬 전환 시 조정할 AudioSource

    public void GoToScene(int sceneIndex)
    {
        StartCoroutine(GoToSceneRoutine(sceneIndex));
    }

    IEnumerator GoToSceneRoutine(int sceneIndex)
    {
        // 음악 소리 줄이기 (AudioSource가 있을 경우에만)
        if (audioSource != null)
        {
            StartCoroutine(FadeOutMusic());
        }

        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

        // 새로운 씬 열기
        SceneManager.LoadScene(sceneIndex);
    }

    public void GoToSceneAsync(int sceneIndex)
    {
        StartCoroutine(GoToSceneAsyncRoutine(sceneIndex));
    }

    IEnumerator GoToSceneAsyncRoutine(int sceneIndex)
    {
        // 음악 소리 줄이기 (AudioSource가 있을 경우에만)
        if (audioSource != null)
        {
            StartCoroutine(FadeOutMusic());
        }

        fadeScreen.FadeOut();

        // 새로운 씬 열기
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        float timer = 0;
        while (timer <= fadeScreen.fadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        operation.allowSceneActivation = true;
    }

    IEnumerator FadeOutMusic()
    {
        float startVolume = audioSource.volume; // 현재 음악 볼륨 저장
        float fadeDuration = 1f; // 음악이 사라지는 시간 (초)
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, timer / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0; // 최종적으로 볼륨을 0으로 설정
    }
}
