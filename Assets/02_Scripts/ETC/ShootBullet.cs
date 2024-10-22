using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class ShootBullet : MonoBehaviour
{
    public GameObject bulletPrefab; // 발사할 총알 프리팹
    public Transform firePoint; // 총알이 발사될 위치
    public float bulletSpeed = 20f; // 총알 속도

    private XRController controller; // VR 컨트롤러

    void Start()
    {
        // XR 컨트롤러 초기화
        controller = GetComponent<XRController>();
    }

    void Update()
    {
        // 컨트롤러의 Trigger 버튼이 눌렸을 때 총알 발사
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerValue) && triggerValue)
        {
            FireBullet();
        }
    }

    void FireBullet()
    {
        // 총알 프리팹 생성
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // 총알에 Rigidbody 추가 및 발사
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = firePoint.forward * bulletSpeed;
    }
}
