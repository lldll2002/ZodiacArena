using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class CardCube : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    // 원래 위치와 회전값 저장
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    // 원하는 크기 설정
    public Vector3 originalScale = new Vector3(1f, 1f, 1f); // 원래 크기
    public Vector3 grabScale = new Vector3(0.1f, 0.1f, 0.1f); // Grab 시 크기

    // 스케일 변경 속도
    public float scaleChangeSpeed = 2.0f;
    // 위치, 회전 복구 속도
    public float returnSpeed = 2.0f;

    void Start()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        // 이벤트에 콜백 메서드 추가
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        // 큐브의 원래 위치, 회전, 크기를 저장
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        transform.localScale = originalScale;
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("Grabbed the cube!");
        StopAllCoroutines(); // 기존 코루틴 중지
        StartCoroutine(ScaleToTarget(grabScale)); // 스케일 변경
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        Debug.Log("Released the cube!");
        StopAllCoroutines(); // 기존 코루틴 중지
        StartCoroutine(ScaleToTarget(originalScale)); // 원래 크기로 복구
        StartCoroutine(ReturnToOriginalPosition()); // 원래 자리로 복구
    }

    // 스케일을 변경하는 코루틴
    private IEnumerator ScaleToTarget(Vector3 targetScale)
    {
        while (Vector3.Distance(transform.localScale, targetScale) > 0.01f)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, Time.deltaTime * scaleChangeSpeed);
            Debug.Log("Current Scale: " + transform.localScale); // 현재 스케일 출력
            yield return null;
        }

        // 정확한 스케일 적용
        transform.localScale = targetScale;
    }

    // 원래 위치와 회전으로 돌아가는 코루틴
    private IEnumerator ReturnToOriginalPosition()
    {
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f || Quaternion.Angle(transform.rotation, originalRotation) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, Time.deltaTime * returnSpeed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRotation, Time.deltaTime * returnSpeed * 100f);
            yield return null;
        }

        // 정확한 위치와 회전 적용
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }
}
