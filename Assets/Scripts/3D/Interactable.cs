using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float interactableRadius = 3f;
    public Transform interactionTransform;
    public bool hasInteraced = false;
    bool isFocused;
    Transform player;

    [Header("Debugksadklfjasld")]
    public bool showDebug = false;
    public Color debugColor = Color.yellow;

    public virtual void Interact()
    {
        PlayerInteraction_3D.OnInteraction = true;
    }

    void Update()
    {
        if (isFocused && !hasInteraced)
        {
            float distance = Vector3.Distance(player.position, interactionTransform.position);
            if (distance <= interactableRadius)
            {
                hasInteraced = true;
                Interact();
            }
        }
        else if (isFocused && hasInteraced)
        {
            float distance = Vector3.Distance(player.position, interactionTransform.position);
            if (distance > (interactableRadius + 1f))
            {
                player.GetComponent<PlayerInteraction_3D>().RemoveFocus();
                OnDefocused();
            }
        }
    }

    public void OnFocused(Transform playerTransform)
    {
        isFocused = true;
        player = playerTransform;
        hasInteraced = false;
    }

    public void OnDefocused()
    {
        isFocused = false;
        player = null;
        hasInteraced = false;
    }

    void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
            interactionTransform = transform;

        if (showDebug)
        {
            Gizmos.color = debugColor;
            Gizmos.DrawWireSphere(interactionTransform.position, interactableRadius);
        }
    }
}