using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class JeweleryItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Equipment item;
    public Image icon;
    private InventoryUI inventoryui;
    public int price;
    public int amounttobuy;
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
        priceText.text = price.ToString() + "$";
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
        if (coins.itemAmount >= price)
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

    public void JeweleryReroll()
    {
        if (coins.itemAmount >= price && item.itemAmount>=1)
        {
            AudioManager.instance.Play("Buying");
            int randomstrength = Random.Range(0, 101);
            int randomdexterity = Random.Range(0, 101);
            int randompower = Random.Range(0, 101);

            if (randomstrength <= 25)
            {
                item.strengthModifier = 1;
                //25%
            }
            if(randomstrength > 25 && randomstrength <= 45)
            {
                item.strengthModifier = 2;
                //20%
            }
            if (randomstrength > 45 && randomstrength <= 60)
            {
                item.strengthModifier = 3;
                //15%
            }
            if (randomstrength > 60 && randomstrength <= 74)
            {
                item.strengthModifier = 4;
                //14%
            }
            if (randomstrength > 74 && randomstrength <= 86)
            {
                item.strengthModifier = 5;
                //12%
            }
            if (randomstrength > 86 && randomstrength <= 94)
            {
                item.strengthModifier = 6;
                //8%
            }
            if (randomstrength > 94)
            {
                item.strengthModifier = 7;
                //6%
            }

         //--------------------------------------------------------------------//

            if (randomdexterity <= 25)
            {
                item.dexterityModifier = 1;
                //25%
            }
            if (randomdexterity > 25 && randomdexterity <= 45)
            {
                item.dexterityModifier = 2;
                //20%
            }
            if (randomdexterity > 45 && randomdexterity <= 60)
            {
                item.dexterityModifier = 3;
                //15%
            }
            if (randomdexterity > 60 && randomdexterity <= 74)
            {
                item.dexterityModifier = 4;
                //14%
            }
            if (randomdexterity > 74 && randomdexterity <= 86)
            {
                item.dexterityModifier = 5;
                //12%
            }
            if (randomdexterity > 86 && randomdexterity <= 94)
            {
                item.dexterityModifier = 6;
                //8%
            }
            if (randomdexterity > 94)
            {
                item.dexterityModifier = 7;
                //6%
            }

         //---------------------------------------------------------------//

            if (randompower <= 25)
            {
                item.powerModifier = 1;
                //25%
            }
            if (randompower > 25 && randompower <= 45)
            {
                item.powerModifier = 2;
                //20%
            }
            if (randompower > 45 && randompower <= 60)
            {
                item.powerModifier = 3;
                //15%
            }
            if (randompower > 60 && randompower <= 74)
            {
                item.powerModifier = 4;
                //14%
            }
            if (randompower > 74 && randompower <= 86)
            {
                item.powerModifier = 5;
                //12%
            }
            if (randompower > 86 && randompower <= 94)
            {
                item.powerModifier = 6;
                //8%
            }
            if (randompower > 94)
            {
                item.powerModifier = 7;
                //6%
            }
            Inventory.instance.RemoveCoins(coins, price);
        }
        else
        {
            NoMoney.SetActive(true);
            Invoke("Warning", 3f);
        }
    }
}
