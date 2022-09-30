using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

     void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Too many inventories");
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    


    public int InventorySpace = 30;
    public GameObject FullInventoryWarning;
    public GameObject LootInfo;
    public Transform LootInfoPlace;

    public TextMeshProUGUI money;
    public List<Item> items = new List<Item>();

    
    private void Start()
    {
        money.text = "0";
    }
    public void Add(Item item, int AmountofItem)
    {
        if(items.Count >= InventorySpace)
        {
            FullInventoryWarning.SetActive(true);
            Invoke("Warning", 3f);
            return;
        }
        if (items.Contains(item) && item.itemAmount < item.maxStack)
        {
            item.itemAmount+= AmountofItem;
        }
        else
        {
            items.Add(item);
            item.itemAmount+= AmountofItem;

        }
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
        GameManager.instance.OnItemsChanged(item);
        if (AmountofItem > 0)
        {
            LootInfoMessage("You gained " + AmountofItem + " " + item.name);
        }
    }
    private void Warning()
    {
        FullInventoryWarning.SetActive(false);
    }

    public void Remove(Item item, int AmountofItem)
    {
        if (items.Contains(item))
        {
            item.itemAmount-= AmountofItem;
        }
        if(item.itemAmount <= 0)
        {
            items.Remove(item);
        }
        if(item.maxStack == 1)
        {
            items.Remove(item);
        }
            

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
        GameManager.instance.OnItemsChanged(item);
    }

    public void AddCoins(Item item, int amount)
    {
        item.itemAmount+=amount;

        money.text = item.itemAmount.ToString();

    }
    public void RemoveCoins(Item item, int amount)
    {
        item.itemAmount -= amount;

        money.text = item.itemAmount.ToString();
    }

    public int GetItemCount(string type)
    {
        int itemcount = 0;

        foreach (Item item in items)
        {
            if(item.name == type)
            {
                itemcount += item.itemAmount;
            }
        }
        return itemcount;
    }

    private void LootInfoMessage(string message)
    {
        GameObject go = Instantiate(LootInfo, LootInfoPlace);

        TextMeshProUGUI textObject = go.GetComponent<TextMeshProUGUI>();
        textObject.text = message;
        textObject.color = Color.black;

        go.transform.SetAsFirstSibling();

        Destroy(go, 3f);
    }

}
