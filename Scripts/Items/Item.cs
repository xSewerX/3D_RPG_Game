using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]


public class Item : ScriptableObject, ItemDescription
{
    new public string name = "New Item";
    public Sprite icon = null;
    public int itemAmount = 0;
    public int maxStack = 0;
    public string Description;
    public int SellPrice;
    public bool isSellable;

    public virtual void Use()
    {
        //Method to be overridden
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this,1);
    }

    public virtual string GetDescription()
    {
        return "<color=#6D6D6D>" + name + "</color=#6D6D6D>" +
            "\n " + Description + 
            "\n Sell Price: " + SellPrice;
    }
}

