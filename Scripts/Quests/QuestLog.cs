using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{

    private static QuestLog instance;
    Inventory inventory;

    [SerializeField]
    private Transform questParent;

    private int maxQuestCount = 8;
    private int currentQuestCount;

    private Quest selected;

    [SerializeField]
    private GameObject QLFullWarning, questRewards, AbandonBtn, questPrefab, Reward1, Reward2, Reward3;
    [SerializeField]
    private TextMeshProUGUI questTitle, questDescription, questObjectives, questCounttxt, rewardText;

    private TextMeshProUGUI ExpAmount, MoneyAmount, ItemAmount;
    private Image ItemImage;

    private List<QuestScript> questScripts = new List<QuestScript>();
    
    private List<Quest> quests = new List<Quest>();

    public static QuestLog MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<QuestLog>();
            }
            return instance;
        }
    }

    public List<Quest> MyQuests
    {
        get
        {
            return quests;
        }
        set
        {
            quests = value;
        }
    }

    private void Awake()
    {
        inventory = Inventory.instance;
    }

    private void Start()
    {
        questCounttxt.text = currentQuestCount + "/" + maxQuestCount;

         ExpAmount = Reward1.GetComponentInChildren<TextMeshProUGUI>();
         MoneyAmount = Reward2.GetComponentInChildren<TextMeshProUGUI>();
         ItemAmount = Reward3.GetComponentInChildren<TextMeshProUGUI>();
         ItemImage = Reward3.GetComponent<Image>();
    }
    public void AcceptQuest(Quest quest)
    {
        if(currentQuestCount < maxQuestCount)
        {
            currentQuestCount++;
            questCounttxt.text = currentQuestCount + "/" + maxQuestCount;

            foreach (CollectObjective obje in quest.MyCollectObjectives)
            {
                // inventory.onItemChangedCallback += obje.UpdateItemCount;

                GameManager.instance.onitemchangedEvent += new OnItemsChanged(obje.UpdateItemCount);
                obje.UpdateItemCount();
            }

            foreach (KillObjective obje in quest.MyKillObjectives)
            {
                GameManager.instance.killConfirmedEvent += new KillConfirmed(obje.UpdateKillCount);
            }
            quests.Add(quest);

            GameObject gameobject = Instantiate(questPrefab, questParent);

            QuestScript qs = gameobject.GetComponent<QuestScript>();
            qs.MyQuest = quest;
            quest.MyQuestScript = qs;

            questScripts.Add(qs);

            gameobject.GetComponent<TextMeshProUGUI>().text = quest.MyTitle;

            CheckCompletion();
        }
        else
        {
            QLFullWarning.SetActive(true);
            Invoke("Warning", 3f);
        }
        
    }
    private void Warning()
    {
        QLFullWarning.SetActive(false);
    }
    public void UpdateSelected()
    {
        ShowDescription(selected);
    }
    public void ShowDescription(Quest quest)
    {
        if (quest != null)
        {
            if (selected != null && selected != quest)
            {
                selected.MyQuestScript.Deselect();
            }

            selected = quest;

            string objectives = string.Empty;
            string title = quest.MyTitle;
            string description = quest.MyDescription;
            
            rewardText.text = "Rewards:";
            AbandonBtn.SetActive(true);
            questRewards.SetActive(true);


            foreach (QuestObjective obj in quest.MyCollectObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount+"\n";
            }
            foreach (QuestObjective obj in quest.MyKillObjectives)
            {
                if(obj.MyCurrentAmount <= obj.MyAmount)
                {
                    objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
                }
                else
                {
                    objectives += obj.MyType + ": " + obj.MyAmount + "/" + obj.MyAmount + "\n";
                }
            }


            questTitle.text = title.ToString();
            questDescription.text = description.ToString();
            questObjectives.text = objectives;
            if (quest.ExpReward <= 0)
            {
                Reward1.SetActive(false);
            }
            else
            {
                ExpAmount.text = quest.ExpReward.ToString();
                Reward1.SetActive(true);
            }
            if (quest.MoneyReward == null)
            {
                Reward2.SetActive(false);
            }
            else
            {
                MoneyAmount.text = quest.MoneyRewardAmount.ToString();
                Reward2.SetActive(true);
            }
            if (quest.itemReward == null)
            {
                Reward3.SetActive(false);
            }
            else
            {
                ItemAmount.text = quest.ItemRewardAmount.ToString();
                ItemImage.sprite = quest.itemReward.icon;
                Reward3.SetActive(true);
            }
        }
    }

    public void CheckCompletion()
    {
        foreach(QuestScript qs in questScripts)
        {
            qs.MyQuest.MyQuestGiver.UpdateQuestStatus();
            qs.IsComplete();
        }
    }

    public void AbandonQuest()
    {
        foreach (CollectObjective objective in selected.MyCollectObjectives)
        {
            //Inventory.instance.onItemChangedCallback -= objective.UpdateItemCount;
            GameManager.instance.onitemchangedEvent -= new OnItemsChanged(objective.UpdateItemCount);
        }
        foreach (KillObjective obje in selected.MyKillObjectives)
        {
            GameManager.instance.killConfirmedEvent -= new KillConfirmed(obje.UpdateKillCount);
            obje.MyCurrentAmount = 0;
        }
        RemoveQuest(selected.MyQuestScript);
    }

    public void RemoveQuest(QuestScript qs)
    {
        questScripts.Remove(qs);
        Destroy(qs.gameObject);
        quests.Remove(qs.MyQuest);

        questTitle.text = string.Empty;
        questDescription.text = string.Empty;
        questObjectives.text = string.Empty;
        Reward1.SetActive(false);
        Reward2.SetActive(false);
        Reward3.SetActive(false);

        rewardText.text = null;
        AbandonBtn.SetActive(false);
        questRewards.SetActive(false);

        selected = null;
        qs.MyQuest.MyQuestGiver.UpdateQuestStatus();
        qs = null;
        currentQuestCount--;
        questCounttxt.text = currentQuestCount + "/" + maxQuestCount;
    }

    public bool HasQuest(Quest quest)
    {
        return quests.Exists(x => x.MyTitle == quest.MyTitle);
    }


}
