using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    public Vector2 movement { get; private set; }
    public bool jumpPressed { get; private set; }
    public bool sprintPressed { get; private set; }

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

        sprintAction = playerInput.actions["sprint"];
        sprintAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        sprintAction.Disable();
    }

    void Update()
    {
        movement = moveAction.ReadValue<Vector2>();
        jumpPressed = jumpAction.triggered;
        sprintPressed = sprintAction.triggered;
    }
}
