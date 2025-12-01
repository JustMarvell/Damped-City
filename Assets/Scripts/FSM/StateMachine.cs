using UnityEngine;
using UnityEngine.InputSystem;

public enum CURRENT_STATE
{
    idle,
    move,
    sprint,
    jump,
    fall,
    land
}

public class StateMachine : MonoBehaviour
{
    public CURRENT_STATE m_current_state;

    [Space]

    private State currentState;

    public CharacterController Controller { get; private set; }
    public Animator Animator { get; private set; }
    public Transform Transform => transform;

    [Header("Movement Settings")]

    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float rotationSmoothTime = 0.1f;
    public float jumpForce = 8f;
    public float gravity = -9.81f;
    public bool allowAirControl = true;
    [Range(0f, 1f)] public float airControlFactor = 0.3f;
    [Range(0f, 0.5f)] public float landingDelay = 0.15f;

    [Header("Ground Check Settings")]
    public float groundCheckRadius = 0.4f;
    public float groundCheckOffset = 0.1f;
    public LayerMask groundMask;

    private float rotationVelocity;

    private PlayerInputHandler inputHandler;
    public Vector2 MovementInput => inputHandler.movement;
    public bool JumpInput => inputHandler.jumpPressed && currentState.CanJump();
    public bool SprintInput => inputHandler.sprintPressed && !IsSprinting;
    [HideInInspector] public bool IsSprinting;

    public Vector3 verticalVelocity;
    public Vector3 jumpStartMoveDirection;

    void Start()
    {
        IsSprinting = false;
        // ChangeState(new IdleState(this));
    }

    void Update()
    {
        ApplyGravity();
        currentState?.HandleInput();
        currentState?.UpdateState();
    }

    void FixedUpdate()
    {
        currentState?.FixedUpdateState();
    }

    public void ChangeState(State newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState?.EnterState();
    }

    private void ApplyGravity()
    {
        if (!IsGrounded())
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }
        else if (verticalVelocity.y < 0f)
        {
            verticalVelocity.y = -1f;       // to prevent sticking
        }

        Controller.Move(verticalVelocity * Time.deltaTime);
    }

    public bool IsGrounded()
    {
        Vector3 spherePosition = transform.position + Vector3.down * (Controller.height / 2f - Controller.radius + groundCheckOffset);
        return Physics.CheckSphere(spherePosition, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (Controller == null) Controller = GetComponent<CharacterController>();
        Vector3 spherePosition = transform.position + Vector3.down * (Controller.height / 2f - Controller.radius + groundCheckOffset);

        if (IsGrounded())
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.yellow;
        }

        Gizmos.DrawSphere(spherePosition, groundCheckRadius);
    }
#endif
}
