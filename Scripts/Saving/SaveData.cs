using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public PlayerData MyPlayerData { get; set; }
    public InventoryData MyInventoryData { get; set; }
    public List<EquipmentData> MyEquipmentData { get; set; }
    public List<QuestData> MyQuestData { get; set; }
    public List<QuestGiverData> MyQuestGiverData { get; set; }
    public DateTime MyDateTime { get; set; }
    public string MyScene { get; set; }

    public SaveData()
    {
        MyInventoryData = new InventoryData();
        MyEquipmentData = new List<EquipmentData>();
        MyQuestData = new List<QuestData>();
        MyQuestGiverData = new List<QuestGiverData>();
        MyDateTime = DateTime.Now;

    }
}

[Serializable]
public class PlayerData
{
    public int MyLevel { get; set; }
    public float MyExp { get; set; }
    public float MyMaxExp { get; set; }
    public int MyStatPoints { get; set; }
    public float MyHP { get; set; }
    public Stat MyMaxHP { get; set; }
    public float MyMana { get; set; }
    public Stat MyMaxMana { get; set; }
    public Stat MyMS { get; set; }
    public Stat MyArmor { get; set; }
    public Stat MyStrength { get; set; }
    public Stat MyDexterity { get; set; }
    public Stat MyPower { get; set; }
    public float MyX { get; set; }
    public float MyY { get; set; }
    public float MyZ { get; set; }
    public float MyPotionTimer { get; set; }
    public float MyPotionStrength { get; set; }
    public float MyPotionDex { get; set; }
    public float MyPotionPower { get; set; }
    public bool MyIsPotion { get; set; }

    public PlayerData(int level, float Exp, float MaxExp, int StatPoints, float HP, Stat MaxHP, float Mana, Stat MaxMana,
        Stat MS, Stat Armor, Stat Strength, Stat Dexterity, Stat Power, Vector3 position, float potionTimer, float potionStrength,
        float potionDex, float potionPower, bool isPotion)
    {
        this.MyLevel = level;
        this.MyExp = Exp;
        this.MyMaxExp = MaxExp;
        this.MyStatPoints = StatPoints;
        this.MyHP = HP;
        this.MyMaxHP = MaxHP;
        this.MyMana = Mana;
        this.MyMaxMana = MaxMana;
        this.MyMS = MS;
        this.MyArmor = Armor;
        this.MyStrength = Strength;
        this.MyDexterity = Dexterity;
        this.MyPower = Power;
        this.MyX = position.x;
        this.MyY = position.y;
        this.MyZ = position.z;
        this.MyPotionTimer = potionTimer;
        this.MyPotionStrength = potionStrength;
        this.MyPotionDex = potionDex;
        this.MyPotionPower = potionPower;
        this.MyIsPotion = isPotion;
    }
}

[Serializable]
public class ItemData
{
    public string MyName{ get; set; }
    public int MyAmount { get; set; }
    public int MyCoinAmount { get; set; }

    public ItemData(string name, int amount, int CoinAmount)
    {
        MyName = name;
        MyAmount = amount;
        MyCoinAmount = CoinAmount;
    }
}


[Serializable]
public class EquipmentData
{
    public string MyName { get; set; }

    public EquipmentData(string name, int slot)
    {
        MyName = name;
    }
}

[Serializable]
public class InventoryData
{
    public List<ItemData> MyItems { get; set; }
    public InventoryData()
    {
        MyItems = new List<ItemData>();
    }
}

[Serializable]
public class QuestData
{
    public string MyQuestTitle { get; set; }
    public string MyDescription { get; set; }
    public CollectObjective[] MyCollectObjectives { get; set; }
    public KillObjective[] MyKillObjectives { get; set; }
    public int MyQuestGiverID { get; set; }
    

    public QuestData(string title, string description, CollectObjective[] collectObjectives, KillObjective[] killObjectives,
        int questGiverID)
    {
        MyQuestTitle = title;
        MyDescription = description;
        MyCollectObjectives = collectObjectives;
        MyKillObjectives = killObjectives;
        MyQuestGiverID = questGiverID;
    }
}
[Serializable]
public class QuestGiverData
{
    public List<string> MyCompletedQuests { get; set; }
    public int MyQuestGiverID { get; set; }
    
    public QuestGiverData(int questGiverID, List<string> completedQuests)
    {
        this.MyQuestGiverID = questGiverID;
        MyCompletedQuests = completedQuests;
    }
}
