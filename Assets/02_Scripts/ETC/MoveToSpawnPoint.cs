using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class AutoMove : MonoBehaviour
{
    public Transform spawnPoint; // 스폰 포인트를 할당할 변수
    public GameObject xrOrigin; // XR Origin을 할당할 변수
    public GameObject lobby; // 활성화할 로비 오브젝트
    public AudioClip narrationClip; // 나레이션 SFX를 할당할 변수
    public float moveDuration = 2.0f; // 이동에 걸리는 시간
    public float waitDuration = 1.0f; // 이동 후 대기할 시간
    public float rotationDuration = 4.0f; // 회전에 걸리는 시간

    private AudioSource audioSource; // 오디오 소스 컴포넌트
    private bool isMoving = false; // 이동 중 여부 체크

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // 오디오 소스 컴포넌트 추가
        audioSource.clip = narrationClip; // 나레이션 클립 설정
        lobby.SetActive(false); // 초기 로비 비활성화
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

        // 360도 회전
        yield return StartCoroutine(Rotate360());

        // 컨트롤러 입력 활성화
        EnableInput();

        // 로비 활성화 및 스크립트 비활성화
        ActivateLobbyAndDisableScript();

        isMoving = false;
    }

    private IEnumerator Rotate360()
    {
        // 나레이션 SFX 재생
        audioSource.Play();

        float elapsedTime = 0f;
        float totalRotation = 360f; // 총 회전각
        float currentRotation = 0f;

        // 회전
        while (currentRotation < totalRotation)
        {
            float rotationThisFrame = (totalRotation / rotationDuration) * Time.deltaTime; // 매 프레임마다 회전할 각도
            xrOrigin.transform.Rotate(0, rotationThisFrame, 0, Space.World); // Y축을 기준으로 회전
            currentRotation += rotationThisFrame;
            yield return null; // 다음 프레임까지 대기
        }

        // 회전 완료 확인 로그
        Debug.Log("360도 회전 완료!");
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

    private void ActivateLobbyAndDisableScript()
    {
        // 로비 활성화
        lobby.SetActive(true);

        // 스크립트 비활성화
        this.enabled = false;

        Debug.Log("로비가 활성화되고 AutoMove 스크립트가 비활성화되었습니다.");
    }
}
