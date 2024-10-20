using UnityEngine;

    // 큐브 클릭 시 비활성화 하는 슼릡트
public class CubeClickHandler : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    void Start()
    {
        // 오브젝트의 메시렌더러 컴포넌트를 가져옴
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void OnObjectClicked()
    {
        // 큐브의 MeshRenderer 비활성화
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
            Debug.Log("메쉬렌더러가 비활성화 되었습니다");
        }
    }
}

