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

        counterText.text = "0 / " + GameMaster.instance.numberOfItemToCollect.ToString();
    }

    void UpdateCounter()
    {
        counterText.text = inventory.CheckCollectedItemNumber(itemToCount).ToString() + " / " + GameMaster.instance.numberOfItemToCollect.ToString();
    }
}
