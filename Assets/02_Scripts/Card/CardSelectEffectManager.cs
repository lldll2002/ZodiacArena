using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class CardSelectEffectManager : MonoBehaviour
{
    public GameObject effectPrefab; // 이펙트 프리팹
    public float vibrationDuration = 0.1f; // 진동 지속 시간
    public float vibrationStrength = 0.5f; // 진동 강도

    // 버튼 클릭 시 호출되는 메소드
    public void OnCardSelected(Button clickedButton)
    {
        // VR 컨트롤러 진동
        VibrateController();

        // 클릭된 버튼의 위치에 이펙트 생성
        CreateEffect(clickedButton);
    }

    private void VibrateController()
    {
        // VR 컨트롤러 진동을 설정하는 코드 (예: Unity XR Toolkit)
        var controller = GetComponent<XRController>();
        if (controller)
        {
            controller.SendHapticImpulse(vibrationStrength, vibrationDuration);
        }
    }

    private void CreateEffect(Button clickedButton)
    {
        // 버튼의 위치를 가져옴
        Vector3 position = clickedButton.transform.position;

        // 이펙트 프리팹을 해당 위치에 생성
        Instantiate(effectPrefab, position, Quaternion.identity);
    }
}
