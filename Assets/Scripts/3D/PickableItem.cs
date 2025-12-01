using UnityEngine;

public class PickableItem : Interactable
{
    public Item item;
    AudioManager audioManager;
    FMODEvents fMODEvents;

    void Start()
    {
        audioManager = AudioManager.instance;
        fMODEvents = FMODEvents.instance;
    }

    public override void Interact()
    {
        base.Interact();

        PickUpItem();
    }

    void PickUpItem()
    {
        bool wasPickedUp = Inventory.instance.Add(item);
        if (wasPickedUp)
        {
            switch (item.item_Type)
            {
                case Item_Type.DEFAULT:
                    audioManager.PlayOneShot(fMODEvents.INTERACTABLE_DefaultInteractingSound, transform.position);
                break;
                case Item_Type.MONEY_BAG:
                    audioManager.PlayOneShot(fMODEvents.INTERACTABLE_MoneyBagPickup, transform.position);
                break;
            }

            Debug.Log("Picked up " + item.itemName);
            Destroy(gameObject);
        }
        else
        {
            audioManager.PlayOneShot(fMODEvents.INTERACTABLE_DefaultInteractingSound, transform.position);
            Debug.Log("Failed to pick " + item.itemName);
        }

        PlayerInteraction_3D.OnInteraction = false;
    }
}
