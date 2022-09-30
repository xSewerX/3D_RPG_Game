using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Recipe : MonoBehaviour
{

    public materials[] Materials;
    public Item CraftedItem;
    public TextMeshProUGUI textCraftAmount;
    [SerializeField]
    private int AmountOfCraftedItem;
    public Image craftedImage;
    public GameObject NoMaterials;
    bool canCraft = true;

    [Serializable]
    public struct materials
    {
        public Item item;
        public int Amount;
        public Image icon;
        public GameObject craftingSlot;
        public TextMeshProUGUI textAmount;
    }
    private void Start()
    {
        Inventory.instance.onItemChangedCallback += OnItemChangedCallback;

        foreach (materials mats in Materials)
        {
            if (mats.item != null)
            {
                mats.icon.sprite = mats.item.icon;
                mats.icon.enabled = true;
                mats.craftingSlot.SetActive(true);
                mats.textAmount.text = mats.item.itemAmount.ToString() + "/" + mats.Amount.ToString();
            }
            if (mats.Amount <= mats.item.itemAmount)
            {
                mats.textAmount.color = Color.green;
            }
            else
            {
                mats.textAmount.color = Color.red;
            }
        }

        if (CraftedItem != null)
        {
            craftedImage.sprite = CraftedItem.icon;
            craftedImage.enabled = true;
            textCraftAmount.text = AmountOfCraftedItem.ToString();
        }
    }

    private void OnItemChangedCallback()
    {
        foreach (materials mats in Materials)
        {
            if (mats.item != null)
            {
                mats.textAmount.text = mats.item.itemAmount.ToString() + "/" + mats.Amount.ToString();
            }
            if (mats.Amount <= mats.item.itemAmount)
            {
                mats.textAmount.color = Color.green;
            }
            else
            {
                mats.textAmount.color = Color.red;
            }
        }
    }

    private bool CanCraft()
    {
        canCraft = true;
        foreach(materials mats in Materials)
        {
            if (mats.Amount <= mats.item.itemAmount)
            {
                continue;
            }
            else
            {
                NoMaterials.SetActive(true);
                Invoke("Warning", 3f);
                canCraft = false;
                break;
            }

        }
        return canCraft;
    }

    public void Craft()
    {
        
        if (CanCraft())
        {
            if(gameObject.tag == "Blacksmith")
            {
                AudioManager.instance.Play("BlacksmithCraft");
            }
            else if(gameObject.tag == "Alchemist")
            {
                AudioManager.instance.Play("AlchemyCraft");
            }
            Inventory.instance.Add(CraftedItem, AmountOfCraftedItem);
            foreach (materials mats in Materials)
            {
                Inventory.instance.Remove(mats.item, mats.Amount);
            }
        }
        
    }
    private void Warning()
    {
        NoMaterials.SetActive(false);
    }
  
}
