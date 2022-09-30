using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;
    public Transform itemsParent;
    public GameObject tooltip;
    public TextMeshProUGUI tooltiptext;
    InventorySlot[] slots;
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }


    public void ShowTooltip(Vector3 position, ItemDescription description)
    {
        tooltip.SetActive(true);
        tooltip.transform.position = position;
        tooltiptext.text = description.GetDescription();
    }
    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
    
}
