using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingCategoryUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Canvas canvas;
    void Start()
    {
        canvas = GetComponent<Canvas>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        canvas.sortingOrder = 4;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canvas.sortingOrder = 2;
    }

}
