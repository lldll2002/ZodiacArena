using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioClip buttonClickSound; // 사용할 오디오 클립을 할당합니다.
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(buttonClickSound);
    }
}
