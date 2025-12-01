using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler2D : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction punchAction;

    public Vector2 movement { get; private set; }
    public bool jumpPressed { get; private set; }
    public bool dashPressed { get; private set; }
    public bool punchPressed { get; private set; }

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void OnEnable()
    {
        moveAction = playerInput.actions["move"];
        moveAction.Enable();

        jumpAction = playerInput.actions["jump"];
        jumpAction.Enable();

        dashAction = playerInput.actions["dash"];
        dashAction.Enable();

        punchAction = playerInput.actions["punch"];
        punchAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        dashAction.Disable();
        punchAction.Disable();
    }

    void Update()
    {
        movement = moveAction.ReadValue<Vector2>();

        jumpPressed = jumpAction.triggered;
        dashPressed = dashAction.triggered;
        punchPressed = punchAction.triggered;
    }
}
