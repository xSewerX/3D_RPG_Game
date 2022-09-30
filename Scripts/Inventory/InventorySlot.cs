using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Item item;
    public Image icon;
    public Button removeButton;
    public TextMeshProUGUI amount;
    EquipmentManager equipment;

    private InventoryUI inventoryui;
    

    private void Start()
    {
        //amount.enabled = false;
        equipment = EquipmentManager.instance;
        inventoryui = FindObjectOfType<InventoryUI>();
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
        
       

        amount.enabled = true;
        amount.text = item.itemAmount.ToString();
        if(item.maxStack == 1)
        {
            amount.text = item.maxStack.ToString();
        }
    }

    public void ClearSlot()
    {
        amount.enabled = false;
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        AudioManager.instance.Play("ButtonClick");
        Inventory.instance.Remove(item,1);
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(icon.enabled == true)
        {
            inventoryui.ShowTooltip(transform.position, item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryui.HideTooltip();
    }
}

