using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager_3D : MonoBehaviour
{
    private PlayerInput input;

    public InputAction clickAction;
    public InputAction mousePositionAction;

    public bool clickInput;
    public Vector2 mousePositionInput;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        clickInput = clickAction.triggered;
        mousePositionInput = mousePositionAction.ReadValue<Vector2>();
    }

    void OnEnable()
    {
        clickAction = input.actions["Click"];
        clickAction.Enable();

        mousePositionAction = input.actions["MousePosition"];
        mousePositionAction.Enable();
    }

    void OnDisable()
    {
        clickAction.Disable();    
        mousePositionAction.Enable();
    }
}
