using UnityEngine;
using System.Collections;
public class CutsceneManager : MonoBehaviour
{
    [Header("Cutscene Settings")]
    [SerializeField] private GameObject player1Zodiac; // 플레이어 1의 별자리 모델
    [SerializeField] private GameObject player2Zodiac; // 플레이어 2의 별자리 모델
    [SerializeField] private float moveDistance = 2.0f; // 부딪힐 때 이동할 거리
    [SerializeField] private float moveSpeed = 1.0f; // 이동 속도
    [SerializeField] private float fallDelay = 1.0f; // 쓰러지는 딜레이

    public void StartCutscene(bool player1Wins)
    {
        StartCoroutine(PlayCutscene(player1Wins));
    }

    private IEnumerator PlayCutscene(bool player1Wins)
    {
        // 두 모델을 서로 이동
        Vector3 targetPosition1 = player1Zodiac.transform.position + Vector3.right * moveDistance;
        Vector3 targetPosition2 = player2Zodiac.transform.position + Vector3.left * moveDistance;

        float elapsedTime = 0;
        Vector3 startPos1 = player1Zodiac.transform.position;
        Vector3 startPos2 = player2Zodiac.transform.position;

        // 모델 이동
        while (elapsedTime < moveDistance / moveSpeed)
        {
            elapsedTime += Time.deltaTime;
            player1Zodiac.transform.position = Vector3.Lerp(startPos1, targetPosition1, elapsedTime / (moveDistance / moveSpeed));
            player2Zodiac.transform.position = Vector3.Lerp(startPos2, targetPosition2, elapsedTime / (moveDistance / moveSpeed));
            yield return null;
        }

        // 결과에 따라 쓰러지기
        if (player1Wins)
        {
            // 플레이어 2가 쓰러짐
            StartCoroutine(FallZodiac(player2Zodiac));
        }
        else
        {
            // 플레이어 1이 쓰러짐
            StartCoroutine(FallZodiac(player1Zodiac));
        }

        // UI 활성화
        yield return new WaitForSeconds(fallDelay);
        EnablePlayerInfoUI();
    }

    private IEnumerator FallZodiac(GameObject zodiac)
    {
        // 쓰러지는 애니메이션
        Quaternion targetRotation = Quaternion.Euler(0, 0, 90); // 90도 회전
        while (Quaternion.Angle(zodiac.transform.rotation, targetRotation) > 0.1f)
        {
            zodiac.transform.rotation = Quaternion.Slerp(zodiac.transform.rotation, targetRotation, Time.deltaTime * 2);
            yield return null;
        }
    }

    private void EnablePlayerInfoUI()
    {
        // PlayerInfo UI 활성화 로직 추가
        // 예를 들어, UI를 활성화하는 코드 작성
    }
}
