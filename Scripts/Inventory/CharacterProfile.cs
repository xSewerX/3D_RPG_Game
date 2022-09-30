using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class CharacterProfile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Item item;
    EquipmentManager equipmentManager;
    public Image icon;
    private InventoryUI inventoryui;
   // public TextMeshProUGUI tooltiptext;

    private void Start()
    {
        inventoryui = FindObjectOfType<InventoryUI>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (icon.enabled == true)
        {
           // inventoryui.ShowTooltip(transform.position);
        }
        //tooltiptext.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // tooltiptext.enabled = false;
        inventoryui.HideTooltip();
    }
}
