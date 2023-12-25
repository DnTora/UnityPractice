using UnityEngine;
using UnityEngine.InputSystem;

public class HumanoidInput : MonoBehaviour
{

    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    public Vector2 LookInput { get; private set; } = Vector2.zero;
    public bool InvertMouseY { get; private set; } = true;
    private HumanoidInputActions _input = null;
    public float MovementSpeed = 30.0f;

    void OnEnable()
    {
        _input = new HumanoidInputActions();
        _input.humanoid.Enable();

        _input.humanoid.Move.performed += SetMove;
        _input.humanoid.Move.canceled += SetMove;

        _input.humanoid.Look.performed += SetLook;
        _input.humanoid.Look.canceled += SetLook;

    }

    void OnDisable()
    {
        _input.humanoid.Move.performed -= SetMove;
        _input.humanoid.Move.canceled -= SetMove;

        _input.humanoid.Look.performed -= SetLook;
        _input.humanoid.Look.canceled -= SetLook;

        _input.humanoid.Disable();
    }

    private void SetLook(InputAction.CallbackContext ctx)
    {
        LookInput = ctx.ReadValue<Vector2>();
    }

    private void SetMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }
}
