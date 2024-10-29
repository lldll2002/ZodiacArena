using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class AutoMove : MonoBehaviour
{
    [Header("SpawnPoint")]
    public Transform spawnPoint; // 스폰 포인트를 할당할 변수
    public GameObject xrOrigin; // XR Origin을 할당할 변수
    public AudioClip narrationClip; // 나레이션 SFX를 할당할 변수

    [Header("Move...Durations")]
    public float moveDuration = 2.0f; // 이동에 걸리는 시간
    public float waitDuration = 1.0f; // 이동 후 대기할 시간
    public float rotationDuration = 4.0f; // 회전에 걸리는 시간

    [Header("Effect")]
    public GameObject effectPrefab; // 생성할 이펙트 프리팹
    public AudioClip effectSFX; // 이펙트 SFX를 할당할 변수
    public Transform[] effectSpawnPoints; // 이펙트가 생성될 스폰 포인트 배열


    private AudioSource audioSource; // 오디오 소스 컴포넌트
    private bool isMoving = false; // 이동 중 여부 체크

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // 오디오 소스 컴포넌트 추가
        audioSource.clip = narrationClip; // 나레이션 클립 설정
        StartCoroutine(MoveToSpawnPoint()); // 게임 시작 시 자동으로 이동
    }

    private IEnumerator MoveToSpawnPoint()
    {
        isMoving = true;

        // 컨트롤러 입력 비활성화
        DisableInput();

        Vector3 startPosition = xrOrigin.transform.position;
        Vector3 destination = spawnPoint.position;

        float elapsedTime = 0f;

        // 이동
        while (elapsedTime < moveDuration)
        {
            xrOrigin.transform.position = Vector3.Lerp(startPosition, destination, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 최종 위치 설정
        xrOrigin.transform.position = destination;

        // 이동이 완료된 후 대기
        yield return new WaitForSeconds(waitDuration);

        // 360도 회전과 이펙트 생성 시작
        yield return StartCoroutine(RotateAndSpawnEffects());

        // 컨트롤러 입력 활성화
        EnableInput();

        // 로비로 이동
        GotoLobby();

        isMoving = false;
    }

    private IEnumerator RotateAndSpawnEffects()
    {
        // 나레이션 SFX 재생
        audioSource.Play();

        float elapsedTime = 0f;
        float totalRotation = 360f; // 총 회전각
        float currentRotation = 0f;

        // 이펙트 생성 타이밍
        int effectSpawnIndex = 0; // 이펙트 스폰 위치 인덱스
        float nextEffectSpawnTime = 1.0f; // 첫 이펙트 생성 시간 설정

        // 회전
        while (currentRotation < totalRotation)
        {

            elapsedTime += Time.deltaTime;

            // 1초마다 이펙트 생성
            if (elapsedTime >= nextEffectSpawnTime && effectSpawnIndex < effectSpawnPoints.Length)
            {
                // 이펙트 생성
                Instantiate(effectPrefab, effectSpawnPoints[effectSpawnIndex].position, Quaternion.identity);

                // 이펙트 SFX 재생
                audioSource.PlayOneShot(effectSFX); // 이펙트에 대한 SFX 재생

                effectSpawnIndex++;
                nextEffectSpawnTime += 1.0f; // 다음 이펙트 생성 시간 설정
            }

            yield return null; // 다음 프레임까지 대기
        }

        // 회전 완료 확인 로그
        Debug.Log("360도 회전 및 이펙트 생성 완료!");
    }

    private void DisableInput()
    {
        foreach (var controller in FindObjectsOfType<XRController>())
        {
            controller.enabled = false; // 모든 XR Controller 비활성화
        }
    }

    private void EnableInput()
    {
        foreach (var controller in FindObjectsOfType<XRController>())
        {
            controller.enabled = true; // 모든 XR Controller 활성화
        }
    }

    private void GotoLobby()
    {
        SceneManager.LoadScene("01_Scenes/02CardGameVR/Lobby");
    }
}
