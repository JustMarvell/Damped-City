using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine;

public class KeyPressTrigger_Helper : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private Key triggerKey = Key.Space;
    
    [Header("Response")]
    [SerializeField] private UnityEvent onKeyPressed = new UnityEvent();

    private void Update()
    {
        // Check for key press using new Input System (polling is efficient for single keys)
        if (Keyboard.current != null && Keyboard.current[triggerKey].wasPressedThisFrame)
        {
            onKeyPressed?.Invoke();
        }
    }
}
