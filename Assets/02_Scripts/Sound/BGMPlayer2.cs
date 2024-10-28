using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BGMPlayer2 : MonoBehaviour
{
    private static BGMPlayer2 instance = null; // 여기에서 BGMPlayer2로 수정
    private AudioSource audioSource; // BGM 재생을 위한 AudioSource

    void Awake()
    {
        // 오브젝트가 이미 존재하는지 확인
        if (instance == null)
        {
            instance = this;
            // 씬 전환 시 파괴되지 않도록 설정
            DontDestroyOnLoad(gameObject);

            // AudioSource 설정
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = Resources.Load<AudioClip>("YourBGMFile"); // 음악 파일 경로 설정
            audioSource.loop = true; // 음악 반복 재생
            audioSource.Play(); // 음악 재생
        }
        else
        {
            // 동일한 오브젝트가 이미 존재하면 새로 생성된 오브젝트를 파괴
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // 현재 씬의 인덱스를 확인하여 특정 씬에 도달하면 BGMPlayer를 파괴
        if (SceneManager.GetActiveScene().buildIndex == 12)
        {
            StartCoroutine(FadeOutAndDestroy(3.0f)); // 3초 동안 페이드 아웃 후 파괴
        }
    }

    private IEnumerator FadeOutAndDestroy(float fadeDuration)
    {
        float startVolume = audioSource.volume;
        float timer = 0;

        // 음악 페이드 아웃
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, timer / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0; // 최종적으로 볼륨을 0으로 설정
        Destroy(gameObject); // BGMPlayer 파괴
    }
}
