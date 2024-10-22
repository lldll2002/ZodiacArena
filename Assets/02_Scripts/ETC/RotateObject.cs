using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // 회전 속도
    public float rotationSpeed = 20f;

    void Update()
    {
        // Y축을 기준으로 회전
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
