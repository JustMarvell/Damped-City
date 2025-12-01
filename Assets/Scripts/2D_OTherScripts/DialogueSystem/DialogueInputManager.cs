using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class DialogueInputManager : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputAction nextDialogueAction;

    public bool nextDialoguePressed { get; private set; }

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        nextDialoguePressed = nextDialogueAction.triggered;

        if (nextDialoguePressed && DialogueManager.isInDialogue)
        {
            DialogueManager.instance.DisplayNextSentences();
        }
    }

    void OnEnable()
    {
        playerInput.enabled = true;

        nextDialogueAction = playerInput.actions["NextDialogue"];
        nextDialogueAction.Enable();
    }

    void OnDisable()
    {
        playerInput.enabled = false;

        nextDialogueAction.Disable();
    }
}
