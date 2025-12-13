using UnityEngine;
using UnityEngine.UI;

public class StateMachine_3D : MonoBehaviour
{
    public CURRENT_STATE_3D m_currentState;

    [Space]

    public CharacterState_3D currentState;
    public CharacterController controller;
    public Animator animator;
    public Transform Transform => transform;

    [Header("Movesdafskdfalskdfj")]
    public float normalSpeed = 5f;
    public float gravity;

    [Header("Sprint Control")]
    public float sprintSpeed = 8f;
    public float maxStamina = 50f;
    public float currentStamina = 0f;
    public float staminaConsumtionRate = 2f;
    public float staminaRegenRate = 4f;
    public Slider staminaSlider;
    

    [Header("Otherasldkfjas")]
    public bool lockCursorOnStart = true;
    public float groundCheckRadius = .4f;
    public float groundCheckOffset = .1f;
    public LayerMask groundMask;
    public LayerMask hidingPlaceMask;
    public Vector2 hidingAngleClamp = new(-20, 20);

    [HideInInspector] public CharacterInputHandler inputHandler;
    public Vector2 MovementInput => inputHandler.movement;
    public bool SprintInput => inputHandler.sprintPressed;
    public bool IsSprinting;
    public bool HidingInput => inputHandler.special1Pressed;

    public static bool IS_INTERACTING = false;
    public static bool IS_DETECTED_BY_ENEMY = false;
    public Vector3 verticalVelocity;
    private Vector3 spherePos;

    [HideInInspector] public BoxCollider bc;
    [HideInInspector] public Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        controller = GetComponent<CharacterController>();
        if (animator == null) animator = GetComponent<Animator>();
        inputHandler = GetComponent<CharacterInputHandler>();
    }

    void Start()
    {
        currentStamina = maxStamina;

        if (staminaSlider != null) staminaSlider.maxValue = maxStamina;

        IsSprinting = false;
        ChangeState(new State_Idle(this));

        if (lockCursorOnStart)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        currentState?.HandleInput();
        currentState?.UpdateState();
    }

    void FixedUpdate()
    {
        ApplyGravity();
        currentState?.FixedUpdateState();


        RegenerateStamina();
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        UpdateStaminaUI();
    }

    void UpdateStaminaUI()
    {
        if (staminaSlider != null)
        {
            if (currentStamina >= maxStamina)
            {
                staminaSlider.gameObject.SetActive(false);
            }
            else if (currentStamina < maxStamina)
            {
                staminaSlider.gameObject.SetActive(true);
                staminaSlider.value = currentStamina;
            }
        }
    }

    void RegenerateStamina()
    {
        if (currentStamina < maxStamina && m_currentState != CURRENT_STATE_3D.SPRINT)
        {
            currentStamina = Mathf.MoveTowards(currentStamina, maxStamina + 30, staminaRegenRate * Time.fixedDeltaTime);
        }
    }

    public void ChangeState(CharacterState_3D newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState?.EnterState();
    }

    private void ApplyGravity()
    {
        if (!IsGrounded())
            verticalVelocity.y += gravity * Time.fixedDeltaTime;
        else if (verticalVelocity.y < 0f)
            verticalVelocity.y = -1f;

        if (m_currentState != CURRENT_STATE_3D.HIDING)
            controller.Move(verticalVelocity * Time.fixedDeltaTime);
    }

    public bool IsGrounded()
    {
        spherePos = transform.position + Vector3.down * (controller.height / 2f - controller.radius + groundCheckOffset);
        return Physics.CheckSphere(spherePos, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();

        if (IsGrounded())
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.yellow;

        Gizmos.DrawSphere(spherePos, groundCheckRadius);
    }
#endif
}

public enum CURRENT_STATE_3D
{
    IDLE,
    MOVE,
    SPRINT,
    INTERACT,
    HIDING
}
