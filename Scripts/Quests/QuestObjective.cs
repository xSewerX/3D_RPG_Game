using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public abstract class QuestObjective
{
    [SerializeField]
    private int amount;
    
    private int currentAmount;
    [SerializeField]
    private string type;

    

    public int MyAmount
    {
        get
        {
            return amount;
        }
    }
    public int MyCurrentAmount
    {
        get
        {
            return currentAmount;
        }
        set
        {
            currentAmount = value;
        }
    }
    public string MyType
    {
        get
        {
            return type;
        }
    }

    public bool IsComplete
    {
        get
        {
            return MyCurrentAmount >= MyAmount;
        }
    }


}

[System.Serializable]
public class CollectObjective : QuestObjective
{

    public void UpdateItemCount(Item item)
    {
        if(MyType.ToLower() == item.name.ToLower())
        {
            MyCurrentAmount = item.itemAmount;
            QuestLog.MyInstance.UpdateSelected();
            QuestLog.MyInstance.CheckCompletion();
            if (MyCurrentAmount > 0 && MyCurrentAmount <= MyAmount)
            {
                QuestMessage.instance.WriteMessage(string.Format("{0}: {1}/{2}", item.name, MyCurrentAmount, MyAmount));
            }
        }
    }
    public void UpdateItemCount()
    {
        MyCurrentAmount = Inventory.instance.GetItemCount(MyType);
        QuestLog.MyInstance.UpdateSelected();
        QuestLog.MyInstance.CheckCompletion();
    }
    public void Complete()
    {
        foreach (Item item in Inventory.instance.items.ToList())
        {
            if(item.name == MyType)
            {
                Inventory.instance.Remove(item, MyAmount);
            }
        }
    }
    
}

[System.Serializable]
public class KillObjective : QuestObjective
{
    public void UpdateKillCount(EnemyStats enemyStats)
    {
        if(MyType == enemyStats.MyType)
        {
            MyCurrentAmount++;
            if(MyCurrentAmount > 0 && MyCurrentAmount <= MyAmount)
            {
                QuestMessage.instance.WriteMessage(string.Format("{0}: {1}/{2}", enemyStats.MyType, MyCurrentAmount, MyAmount));
            }
            QuestLog.MyInstance.UpdateSelected();
            QuestLog.MyInstance.CheckCompletion();
        }
    }
}
