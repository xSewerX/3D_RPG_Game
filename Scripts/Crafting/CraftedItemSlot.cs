using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftedItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Recipe recipe;
    private InventoryUI inventoryui;

    void Start()
    {
        recipe = GetComponentInParent<Recipe>();
        inventoryui = FindObjectOfType<InventoryUI>();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (recipe.CraftedItem != null)
        {
            inventoryui.ShowTooltip(transform.position, recipe.CraftedItem);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryui.HideTooltip();
    }
}
