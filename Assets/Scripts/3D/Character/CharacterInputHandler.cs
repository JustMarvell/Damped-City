using Cinemachine;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CharacterInputHandler : MonoBehaviour
{
    private PlayerInput input;

    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction interactAction;
    private InputAction special1Action;
    private InputAction special2Action;

    public Vector2 movement { get; private set; }
    public bool sprintPressed { get; private set; }
    public bool interactPressed { get; private set; }
    public bool special1Pressed { get; private set; }
    public bool special2Pressed { get; private set; }

    // public CinemachineVirtualCamera vc;
    // public Slider mouseSensSlider;

    // [Range(1, 300)]
    // public int mouseSensitivity = 70;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
        // mouseSensSlider.value = mouseSensitivity;
    }

    void Start()
    {
        // C_CameraController.instance.sensitivity = mouseSensitivity;
    }

    void Update()
    {
        movement = moveAction.ReadValue<Vector2>();
        sprintPressed = sprintAction.IsPressed();
        interactPressed = interactAction.triggered;
        special1Pressed = special1Action.triggered;
        special2Pressed = special2Action.triggered;

        // mouseSensitivity = (int)mouseSensSlider.value;

        // if (GameMaster.IsPaused)
        // {
        //     C_CameraController.instance.sensitivity = mouseSensitivity;
        // }
    }

    void OnEnable()
    {
        moveAction = input.actions["Move"];
        sprintAction = input.actions["Sprint"];
        interactAction = input.actions["Interact"];
        special1Action = input.actions["Special1"];
        special2Action = input.actions["Special2"];

        moveAction.Enable();
        sprintAction.Enable();
        interactAction.Enable();
        special1Action.Enable();
        special2Action.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        sprintAction.Disable();
        interactAction.Disable();
        special1Action.Disable();
        special2Action.Disable();
    }

    // public void ChangeSens()
    // {
    //     C_CameraController.instance.sensitivity = mouseSensitivity;
    // }
}
