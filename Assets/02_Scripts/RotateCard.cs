using UnityEngine;
using UnityEngine.InputSystem;

public class RotateCard : MonoBehaviour
{
    public InputActionReference dragAction;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dragAction.action.Enable();

        dragAction.action.performed += ctx =>
        {
            Debug.Log(ctx.ToString());
        };
    }

    // Update is called once per frame
    void LateUpdate()
    {

    }
}
