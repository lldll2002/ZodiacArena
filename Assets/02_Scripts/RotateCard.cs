using UnityEngine;
using UnityEngine.InputSystem;

public class RotateCard : MonoBehaviour
{
    public InputActionReference dragAction;
    public Transform targetObject; // 회전시킬 오브젝트를 지정할 변수
    public float rotationSpeed = 100f; // 회전 속도 조절을 위한 변수

    void Start()
    {
        dragAction.action.Enable();
        dragAction.action.performed += OnRotate;
    }

    private void OnRotate(InputAction.CallbackContext context)
    {
        Vector2 joystickInput = context.ReadValue<Vector2>();

        if (targetObject != null)
        {
            // x 값만 사용해서 회전하도록 설정
            float rotationAmount = joystickInput.x * rotationSpeed * Time.deltaTime;
            targetObject.Rotate(Vector3.up, rotationAmount);
        }
        else
        {
            Debug.LogWarning("Target Object가 지정되지 않았습니다.");
        }
    }

    void OnDestroy()
    {
        dragAction.action.performed -= OnRotate;
    }
}
