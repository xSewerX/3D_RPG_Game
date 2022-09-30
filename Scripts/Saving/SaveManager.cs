using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField]
    private Equipment startingSword;
    [SerializeField]
    private Item[] items;
    [SerializeField]
    private Item coin;
    [SerializeField]
    private SavedGame[] saveSlots;
    [SerializeField]
    private GameObject warning;
    [SerializeField]
    private TextMeshProUGUI warningText;

    private string action;
    private SavedGame current;

    private void Awake()
    {
        instance = this;

        
    }

    private void Start()
    {
        foreach (SavedGame saved in saveSlots)
        {
            ShowSavedFiles(saved);
        }
        if (PlayerPrefs.HasKey("Load"))
        {
            Load(saveSlots[PlayerPrefs.GetInt("Load")]);
            PlayerPrefs.DeleteKey("Load");
        }
        else
        {
           foreach (Item item in Inventory.instance.items.ToList())
            {
                Inventory.instance.Remove(item, item.itemAmount);
            }

            foreach (Item item in items)
            {
                if (item.itemAmount > 0)
                {
                    item.itemAmount = 0;
                }
            }
            if (coin.itemAmount > 0)
            {
                coin.itemAmount = 0;
            }
            EquipmentManager.instance.Equip(startingSword);
            CharacterStats.instance.OnEquipmentChanged(startingSword, null);
        }

        
        //Debug.Log(Application.persistentDataPath);
        //C:/Users/Sewer/AppData/LocalLow/DefaultCompany/3D_RPG
    }


    public void ShowWarning(GameObject gameObject)
    {
        action = gameObject.name;
        AudioManager.instance.Play("ButtonClick");
        switch (action)
        {
            case "LoadSave":
                warningText.text = "Load game?";
                break;
            case "SaveGame":
                warningText.text = "Save game?";
                break;
            case "DeleteSave":
                warningText.text = "Delete savefile?";
                break;
        }

        current = gameObject.GetComponentInParent<SavedGame>();
        warning.SetActive(true);
    }
    public void ExecuteAction()
    {
        AudioManager.instance.Play("ButtonClick");
        switch (action)
        {
            case "LoadSave":
                LoadScene(current);
                break;
            case "SaveGame":
                Save(current);
                break;
            case "DeleteSave":
                Delete(current);
                break;
        }
        CloseWarning();
    }

    private void LoadScene(SavedGame savedGame)
    {
        if (File.Exists(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            PlayerPrefs.SetInt("Load", savedGame.index);
            SceneManager.LoadScene(data.MyScene);
            Time.timeScale = 1;
        }
    }

    public void CloseWarning()
    {
        warning.SetActive(false);
    }

    private void Delete(SavedGame savedGame)
    {
        File.Delete(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat");
        savedGame.HideVisuals();

    }
    private void ShowSavedFiles(SavedGame savedGame)
    {
        if (File.Exists(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            savedGame.ShowInfo(data);
        }
    }


    public void Save(SavedGame savedGame)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name +".dat", FileMode.Create);

            SaveData data = new SaveData();

            data.MyScene = SceneManager.GetActiveScene().name;

            SaveInventory(data);
            SaveEquipment(data);
            SavePlayer(data);
            SaveQuests(data);
            SaveQuestGivers(data);

            bf.Serialize(file, data);
            file.Close();

            ShowSavedFiles(savedGame);

        }
        catch (System.Exception)
        {
            Delete(savedGame);
            PlayerPrefs.DeleteKey("Load");

            throw;
        }
    }

    private void SavePlayer(SaveData data)
    {
        data.MyPlayerData = new PlayerData(CharacterStats.instance.Level, CharacterStats.instance.currentExp, CharacterStats.instance.MaxExp, CharacterStats.instance.statPoints,
        CharacterStats.instance.currentHealth, CharacterStats.instance.MaxHealth, CharacterStats.instance.currentMana, CharacterStats.instance.MaxMana, CharacterStats.instance.MovementSpeed,
            CharacterStats.instance.Armor, CharacterStats.instance.Strength, CharacterStats.instance.Dexterity, CharacterStats.instance.Power, CharacterStats.instance.transform.position,
            CharacterStats.instance.PotionTime, CharacterStats.instance.potionStrength, CharacterStats.instance.potionDex, CharacterStats.instance.potionPower, CharacterStats.instance.isPotion);
        
    }

    private void SaveEquipment(SaveData data)
    {
        foreach (Equipment equipment in EquipmentManager.instance.currentEquipment)
        {
            if(equipment != null)
            {
                data.MyEquipmentData.Add(new EquipmentData(equipment.name, EquipmentManager.instance.slotindex));
            }
        }
    }

    private void SaveInventory(SaveData data)
    {
        foreach (Item item in Inventory.instance.items)
        {
            data.MyInventoryData.MyItems.Add(new ItemData(item.name, item.itemAmount, coin.itemAmount));
        }
    }

    private void SaveQuests(SaveData data)
    {
        foreach (Quest quest in QuestLog.MyInstance.MyQuests)
        {
            data.MyQuestData.Add(new QuestData(quest.MyTitle, quest.MyDescription, quest.MyCollectObjectives, quest.MyKillObjectives,
                quest.MyQuestGiver.MyQuestGiverID));
        }
    }

    private void SaveQuestGivers(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach (QuestGiver questGiver in questGivers)
        {
            data.MyQuestGiverData.Add(new QuestGiverData(questGiver.MyQuestGiverID, questGiver.MyCompletedQuests));
        }
    }

    public void Load(SavedGame savedGame)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);

            SaveData data = (SaveData)bf.Deserialize(file);
            
            file.Close();

            LoadEquipment(data);
            LoadInventory(data);
            LoadPlayer(data);
            LoadQuests(data);
            LoadQuestGivers(data);
        }
        catch (System.Exception)
        {
            Debug.Log("Loading Error");
            throw;
        }
    }

    private void LoadPlayer(SaveData data)
    {
        CharacterStats.instance.Level = data.MyPlayerData.MyLevel;
        CharacterStats.instance.currentExp = data.MyPlayerData.MyExp;
        CharacterStats.instance.MaxExp = data.MyPlayerData.MyMaxExp;
        CharacterStats.instance.statPoints = data.MyPlayerData.MyStatPoints;
        CharacterStats.instance.currentHealth = data.MyPlayerData.MyHP;
        CharacterStats.instance.MaxHealth = data.MyPlayerData.MyMaxHP;
        CharacterStats.instance.currentMana = data.MyPlayerData.MyMana;
        CharacterStats.instance.MaxMana = data.MyPlayerData.MyMaxMana;
        CharacterStats.instance.MovementSpeed = data.MyPlayerData.MyMS;
        CharacterStats.instance.Armor = data.MyPlayerData.MyArmor;
        CharacterStats.instance.Strength = data.MyPlayerData.MyStrength;
        CharacterStats.instance.Dexterity = data.MyPlayerData.MyDexterity;
        CharacterStats.instance.Power = data.MyPlayerData.MyPower;
        CharacterStats.instance.PotionTime = data.MyPlayerData.MyPotionTimer;
        CharacterStats.instance.potionStrength = data.MyPlayerData.MyPotionStrength;
        CharacterStats.instance.potionDex = data.MyPlayerData.MyPotionDex;
        CharacterStats.instance.potionPower = data.MyPlayerData.MyPotionPower;
        CharacterStats.instance.isPotion = data.MyPlayerData.MyIsPotion;
        CharacterStats.instance.transform.position = new Vector3(data.MyPlayerData.MyX, data.MyPlayerData.MyY, data.MyPlayerData.MyZ);
        Physics.SyncTransforms();
        CharacterStats.instance.UpdateStats();
    }

    private void LoadEquipment(SaveData data)
    {
        foreach (EquipmentData equipmentData in data.MyEquipmentData)
        {
            EquipmentManager.instance.Equip(Array.Find(items, x => x.name == equipmentData.MyName) as Equipment);
        }
    }

    private void LoadInventory(SaveData data)
    {
        foreach (ItemData itemData in data.MyInventoryData.MyItems)
        {
            Item item = Array.Find(items, x => x.name == itemData.MyName);
            Inventory.instance.Add(item, 0);
            Inventory.instance.AddCoins(coin, 0);

            item.itemAmount = itemData.MyAmount;
            coin.itemAmount = itemData.MyCoinAmount;
            Inventory.instance.onItemChangedCallback();
        }
        
    }

    private void LoadQuests(SaveData data)
    {

        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach (QuestData questData in data.MyQuestData)
        {
            QuestGiver qg = Array.Find(questGivers, x => x.MyQuestGiverID == questData.MyQuestGiverID);
            Quest quest = Array.Find(qg.MyQuests, x => x.MyTitle == questData.MyQuestTitle);
            quest.MyQuestGiver = qg;
            quest.MyKillObjectives = questData.MyKillObjectives;

            QuestLog.MyInstance.AcceptQuest(quest);
            
        }
    }
    private void LoadQuestGivers(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach (QuestGiverData questGiverData in data.MyQuestGiverData)
        {
            QuestGiver questGiver = Array.Find(questGivers, x => x.MyQuestGiverID == questGiverData.MyQuestGiverID);
            questGiver.MyCompletedQuests = questGiverData.MyCompletedQuests;
            questGiver.UpdateQuestStatus();
        }
    }
}
