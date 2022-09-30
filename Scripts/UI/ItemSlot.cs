using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    public Image icon;
    private InventoryUI inventoryui;
    public int price;
    public int amounttobuy;
    public TextMeshProUGUI AmountText;
    public TextMeshProUGUI priceText;
    public Item coins;
    public GameObject NoMoney;
    

    private void Awake()
    {
        inventoryui = FindObjectOfType<InventoryUI>();
    }
    private void Start()
    {
        icon.enabled = true;
        icon.sprite = item.icon;
        priceText.text = price.ToString()+"$";
        AmountText.text = amounttobuy.ToString();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryui.ShowTooltip(transform.position, item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryui.HideTooltip();
    }

    public void BuyButton()
    {
        if(coins.itemAmount >= price)
        {
            AudioManager.instance.Play("Buying");
            Inventory.instance.Add(item, amounttobuy);
            Inventory.instance.RemoveCoins(coins, price);
        }
        else
        {
            NoMoney.SetActive(true);
            Invoke("Warning", 3f);
        }
    }

    private void Warning()
    {
        NoMoney.SetActive(false);
    }
    
}
