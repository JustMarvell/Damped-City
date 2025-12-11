using UnityEngine;
using TMPro;

public class ItemCounter : MonoBehaviour
{
    Inventory inventory;
    public Item itemToCount;
    TextMeshProUGUI counterText;

    void Start()
    {
        inventory = Inventory.instance;

        inventory.onItemChangedCallback += UpdateCounter;

        counterText = GetComponent<TextMeshProUGUI>();

        UpdateCounter();
    }

    void UpdateCounter()
    {
        counterText.text = inventory.CheckCollectedItemNumber(itemToCount).ToString() + " / " + GameMaster.instance.numberOfItemToCollect.ToString();
    }
}
