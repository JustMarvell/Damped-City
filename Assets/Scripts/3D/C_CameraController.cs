using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class C_CameraController : MonoBehaviour
{
    public Transform cam;
    public float sensitivity = 100f;
    public string mouseSensKey = "Mouse_Sens";
    public Vector2 angleClamp = new(-90, 90);
    public Vector2 defaultAngleClamp = new();
    public InputAction mouseAction;
    public Vector2 mouseInput;
    public float rotationX;
    public float rotationY;
    public bool isHiding = false;

    public static C_CameraController instance;

    void Awake()
    {
        instance = this;
        defaultAngleClamp = angleClamp;
    }

    public void SetNewAngleClamp(Vector2 newAngleClamp)
    {
        angleClamp = newAngleClamp;
    }

    public void ResetAngleClamp()
    {
        angleClamp = defaultAngleClamp;
    }

    void Start()
    {
        Vector3 rot = cam.rotation.eulerAngles;

        rotationY = rot.y;
        rotationX = rot.x;

        sensitivity = PlayerPrefs.HasKey(mouseSensKey) ? PlayerPrefs.GetFloat(mouseSensKey) : 100f;
    }

    public void TriggerHiding(Quaternion newRotation)
    {
        isHiding = true;
        cam.position = transform.position + new Vector3(0, 1, 0);
        cam.rotation = newRotation;
    }

    public void TriggerUnhide()
    {
        isHiding = false;
    }

    public void LookAtTarget(Transform target)
    {
        cam.LookAt(target);
    }

    void FixedUpdate()
    {
        if (GameMaster.IsPaused || PlayerInteraction_3D.OnInteraction || isHiding)
            return;
        
        mouseInput = mouseAction.ReadValue<Vector2>() * Time.fixedDeltaTime;

        rotationY += mouseInput.x * sensitivity * Time.fixedDeltaTime;
        rotationX += mouseInput.y * sensitivity * Time.fixedDeltaTime;

        rotationX = Mathf.Clamp(rotationX, angleClamp.x, angleClamp.y);

        Quaternion rotation = Quaternion.Euler(-rotationX, rotationY, 0.0f);
        cam.rotation = rotation;

        cam.position = transform.position + new Vector3(0, 1, 0);
    }

    public float SetMouseSens
    {
        set
        {    
            sensitivity = value;
            PlayerPrefs.SetFloat(mouseSensKey, value);
        }
    }

    void OnEnable()
    {
        mouseAction.Enable();
    }

    void OnDisable()
    {
        mouseAction.Disable();
    }
}
