using UnityEngine;
using Unity.XR.CoreUtils;  // XR Origin을 위한 네임스페이스
using Photon.Pun;          // Photon 네트워크 관련 네임스페이스

public class SetParentXR : MonoBehaviour
{
    private PhotonView photonView;      // PhotonView를 사용하여 네트워크에서 동기화
    private XROrigin xrOrigin;          // XR Origin을 저장할 변수

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        // 네트워크에서 자신의 오브젝트만 처리하기 위해 확인
        if (photonView.IsMine)
        {
            // 씬 내에서 XR Origin 찾기
            xrOrigin = FindObjectOfType<XROrigin>();  // XR Origin을 찾기

            if (xrOrigin != null)
            {
                // XR Origin을 Player의 자식으로 설정
                xrOrigin.transform.SetParent(this.transform);

                // 위치와 회전값을 초기화
                xrOrigin.transform.localPosition = Vector3.zero;
                xrOrigin.transform.localRotation = Quaternion.identity;

                Debug.Log("XR Origin이 Player의 자식으로 설정되었습니다.");
            }
            else
            {
                Debug.LogWarning("XR Origin을 찾을 수 없거나 이미 자식으로 설정되었습니다.");
            }
        }
    }
}
