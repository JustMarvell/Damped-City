using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName = "Item";
    public Item_Type item_Type = Item_Type.DEFAULT;

    public GameObject itemPrefab;
    public Sprite icon = null;
    public bool isDefaultItem = false;
    

    public virtual void Use()
    {
        Debug.Log("Using :" + itemName);
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }
}

public enum Item_Type
{
    DEFAULT,
    MONEY_BAG
}