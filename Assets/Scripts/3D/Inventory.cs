using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public int space = 20;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    public bool isOpeningInventory = false;

    public List<Item> items = new();

    void Awake()
    {
        instance = this;
    }

    public bool Add(Item item)
    {
        if (!item.isDefaultItem)
        {
            if (items.Count >= space)
            {
                print("Inventory full");
                return false;
            }

            items.Add(item);

            onItemChangedCallback?.Invoke();
        }

        return true;
    }

    public bool CheckItem(Item item)
    {
        foreach (Item _item in items)
        {
            if (_item.itemName == item.itemName)
            {
                return true;
            }
        }

        return false;
    }

    public int CheckCollectedItemNumber(Item item)
    {
        int number = 0;
        foreach (Item _item in items)
        {
            if (_item.itemName == item.itemName)
                number++;
        }
        return number;
    }

    public Item GetItem(Item item)
    {
        return items.Find(_item => _item.itemName == item.itemName);
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        onItemChangedCallback?.Invoke();
    }
}

