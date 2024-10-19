using UnityEngine;

//08.19(월)
//임의로 화면에 표시만 되도록 만든다.
//실제 게임에는 영향을 미치지 않는 스크립트이다.
public class MyGizmoss : MonoBehaviour
{
    public Color _color = Color.green;

    [Range(0.1f, 2.0f)] public float _radius = 0.3f;

    void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
