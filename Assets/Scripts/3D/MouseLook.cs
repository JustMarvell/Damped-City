using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 100f; // Adjust this value for sensitivity
    public Vector2 lookClamp = new(-90, 90);
    float xRotation = 0f;

    public InputAction mouseAction;
    public Vector2 mouseInput;

    public static MouseLook instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Lock the cursor to the center of the screen and hide it
        // Cursor.lockState = CursorLockMode.Locked;
        mouseAction.Enable();
    }

    void Update()
    {
        // Get mouse input for horizontal and vertical movement
        mouseInput = mouseSensitivity * Time.deltaTime * mouseAction.ReadValue<Vector2>();

        // Rotate the camera up and down (pitch)
        // xRotation -= mouseInput.y;
        // xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp vertical rotation to prevent flipping
        // transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // // Rotate the player body left and right (yaw)
        // playerBody.Rotate(Vector3.up * mouseX);

        mouseInput.y = Mathf.Clamp(mouseInput.y, lookClamp.x, lookClamp.y);
        transform.rotation *= Quaternion.Euler(mouseInput.y, mouseInput.x, 0);
    }
}
