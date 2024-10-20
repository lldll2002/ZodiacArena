using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    // 큐브 클릭 시 비활성화 하는 슼릡트
    public class CubeClickHandler : MonoBehaviour
    {
        void OnMouseDown()
        {
            // 큐브의 MeshRenderer 비활성화
            MeshRenderer cubeRenderer = GetComponent<MeshRenderer>();
            if (cubeRenderer != null)
            {
                cubeRenderer.enabled = false;
            }
        }
    }
}
