using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction_3D : MonoBehaviour
{
    public GameObject takeItemUIIndicator;
    public GameObject hideUIIndicator;

    [Space]

    public static bool OnInteraction;

    public LayerMask interactionLayer;
    public LayerMask hidingPlaceLayer;
    public float interactionRadius = 3f;

    public Interactable focus;

    private CharacterInputHandler inputHandler;
    private bool InteractInput => inputHandler.interactPressed;

    Interactable interactable;
    float[] dist;

    public bool _onInteraction;

    void Awake()
    {
        inputHandler = GetComponent<CharacterInputHandler>();
    }

    void Update()
    {
        if (!OnInteraction && InteractInput)
        {
            TriggerInteract();
        }

        _onInteraction = OnInteraction;
    }

    void FixedUpdate()
    {
        if(hideUIIndicator != null) hideUIIndicator.SetActive(CheckHidingPlace());
        if(takeItemUIIndicator != null) takeItemUIIndicator.SetActive(CheckItem()); 
    }

    bool CheckItem()
    {
        return Physics.CheckSphere(transform.position, interactionRadius, interactionLayer);
    }

    bool CheckHidingPlace()
    {
        return Physics.CheckSphere(transform.position, interactionRadius, hidingPlaceLayer);
    }

    public void TriggerInteract()
    {
        Collider[] coll = Physics.OverlapSphere(transform.position, interactionRadius, interactionLayer);
        dist = new float[coll.Length];

        for (int i = 0; i < coll.Length; i++)
        {
            dist[i] = Vector3.Distance(transform.position, coll[i].transform.position);
            if (i == coll.Length - 1)
            {
                interactable = null;
                float lv = dist.Min();
                for (int l = 0; l < dist.Length; l++)
                {
                    if (dist[l] == lv)
                    {
                        interactable = coll[l].gameObject.GetComponent<Interactable>();
                        if (interactable != null)
                        {
                            SetFocus(interactable);
                            Invoke(nameof(RemoveFocus), .2f);
                        }

                        break;
                    }
                }
            }
        }

    }
    void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if (focus != null)
                focus.OnDefocused();

            focus = newFocus;
        }
        newFocus.OnFocused(transform);
    }

    public void RemoveFocus()
    {
        if (focus != null)
            focus.OnDefocused();

        interactable = null;
        focus = null;
    }
}
