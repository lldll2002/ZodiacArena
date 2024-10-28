using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class CubeDOT : MonoBehaviour
{

    public InputActionReference holdAction;
    private bool isHolding = false;
    public float rotationSpeed = 90f; // 회전 속도 (각도/초)
    private Tween rotationTween; // DOTween 애니메이션을 저장할 변수

    void Start()
    {
        // transform.DOMove(Vector3.up, 5);
        // transform.DOScale(Vector3.one * 3, 5);

        // DOTween.Init(false, true, LogBehaviour.Verbose).SetCapacity(200, 50);
        /* 
            autoKillMode : 한 번 사용한 DOTween 을 다시 재사용 할 지 결정. 기본값 false
            메모리를 아낄 수 있으나, 의도하지 않은 동작을 유발 할 수 있음
        
            useSafeMod : 약간 느리지만 실행되는 동안, 실행 대상이 파괴 될 경우와 같이 예외 상황을 자동으로 처리
            기본값 true

            LogBehaviour : 오류메시지 기록을 설정합니다.

            .SetCapacity(Tweener 개수, Sequence 개수)
            Tweener 와 Sequence의 사용 할 용량을 설정합니다.

            위의 설정은 기본이기 때문에 따로 써주지 않아도 됨
            각 함수 사용할 때 조절할 수 있음
        */

        /*
            transform.DOMoveX(end value, duration)
            transform.DOMoveX(100, 1)
            목적지 까지 가는데, ~초의 시간만큼
            DOMove 뒤에 X,Y,Z 로 한 축만 움직일 수 있음
            DORotate
        */


        // Primary2DAxis [LeftHand XR Controller] << 왼쪽 스틱 이름
        // Oculus Touch Controller (openXR) > thumbstick


        // transform.DORotate(Vector3.up, 5f, RotateMode.FastBeyond360);

        transform.DORotate(new Vector3(0, 1080, 0), 1.5f, RotateMode.FastBeyond360);

        // holdAction.action.Enable();

        // holdAction.action.performed += ctx =>
        // {
        //     StartHold();
        //     Debug.Log(ctx.ToString());
        // };

        // holdAction.action.performed += ctx => StopHold();
    }

    // void Update()
    // {
    //     if (isHolding)
    //     {
    //         RotateObject();
    //     }
    // }

    // private void StartHold()
    // {
    //     isHolding = true;
    // }

    // private void StopHold()
    // {
    //     isHolding = false;
    // }

    // private void RotateObject()
    // {
    //     // 현재 y축에서 지정한 각도만큼 회전
    //     transform.DORotate(Vector3.right * rotationSpeed, 1f, RotateMode.LocalAxisAdd)
    //              .SetEase(Ease.Linear) // 
    //              .SetLoops(-1, LoopType.Incremental); // -1로 무한 루프
    // }

    // private void StopRotate()
    // {
    //     // 이벤트 등록 해제
    //     holdAction.action.performed -= ctx => StartHold();
    //     holdAction.action.canceled -= ctx => StopHold();
    // }
}
