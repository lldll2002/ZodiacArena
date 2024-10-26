using UnityEngine;
using UnityEngine.InputSystem;

public class RotateCard : MonoBehaviour
{
    public InputActionReference dragAction;


    void Start()
    {
        dragAction.action.Enable();

        dragAction.action.performed += ctx =>
        {
            Debug.Log(ctx.ToString());
        };
    }

    void LateUpdate()
    {

    }
}
