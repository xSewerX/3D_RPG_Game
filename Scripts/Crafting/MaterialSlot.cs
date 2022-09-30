using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MaterialSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Recipe recipe;
    public int RecipeMaterialElement;
    private InventoryUI inventoryui;

    void Start()
    {
        recipe = GetComponentInParent<Recipe>();
        inventoryui = FindObjectOfType<InventoryUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (recipe.Materials[RecipeMaterialElement].icon.enabled == true)
        {
            inventoryui.ShowTooltip(transform.position, recipe.Materials[RecipeMaterialElement].item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryui.HideTooltip();
    }
}
