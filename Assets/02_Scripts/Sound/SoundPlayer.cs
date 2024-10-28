using System.Collections;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource audioSource; // 효과음이 있는 AudioSource를 연결하세요.

    void Start()
    {
        // 1초 뒤에 PlaySound 코루틴을 실행합니다.
        StartCoroutine(PlaySoundAfterDelay(0.2f));
    }

    IEnumerator PlaySoundAfterDelay(float delay)
    {
        // 지정된 시간 동안 대기합니다.
        yield return new WaitForSeconds(delay);

        // 효과음을 재생합니다.
        audioSource.Play();
    }
}
