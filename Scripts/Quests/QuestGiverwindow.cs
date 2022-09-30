using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiverwindow:  MonoBehaviour
{
    #region Singleton
    public static QuestGiverwindow instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion


    [SerializeField]
    private GameObject questPrefab, descriptionWindow, AcceptBtn, CompleteBtn, Reward1, Reward2, Reward3;
    [SerializeField]
    private Transform questParent;
    [SerializeField]
    private TextMeshProUGUI questTitle, questDescription, questObjectives;

    private TextMeshProUGUI ExpAmount, MoneyAmount, ItemAmount;
    private Image ItemImage;

    private QuestGiver questGiver;
    private Quest selectedQuest;
    private List<GameObject> quests = new List<GameObject>();
    

    private void Start()
    {
        ExpAmount = Reward1.GetComponentInChildren<TextMeshProUGUI>();
        MoneyAmount = Reward2.GetComponentInChildren<TextMeshProUGUI>();
        ItemAmount = Reward3.GetComponentInChildren<TextMeshProUGUI>();
        ItemImage = Reward3.GetComponent<Image>();
    }
    public void ShowQuests(QuestGiver questGiver)
    {
        if (questGiver.MyQuests != null)
        {
            this.questGiver = questGiver;

            descriptionWindow.SetActive(false);
            questParent.gameObject.SetActive(true);

            foreach (GameObject go in quests)
            {
                Destroy(go);
            }
            foreach (Quest quest in questGiver.MyQuests)
            {
                if (quest != null)
                {
                    GameObject gameobject = Instantiate(questPrefab, questParent);

                    gameobject.GetComponent<QuestScriptNPC>().MyQuest = quest;

                    gameobject.GetComponent<TextMeshProUGUI>().text = quest.MyTitle;

                    quests.Add(gameobject);

                    if (QuestLog.MyInstance.HasQuest(quest))
                    {
                        Color c = gameobject.GetComponent<TextMeshProUGUI>().color;
                        c.a = 0.3f;


                        if (quest.IsComplete)
                        {
                            c = Color.green;
                        }
                        gameobject.GetComponent<TextMeshProUGUI>().color = c;
                    }
                }
            }
        }
    }
    
    public void ShowDescription(Quest quest)
    {
        descriptionWindow.SetActive(true);
        questParent.gameObject.SetActive(false);

        if (QuestLog.MyInstance.HasQuest(quest))
        {
            AcceptBtn.SetActive(false);
        }
        else
        {
            AcceptBtn.SetActive(true);
        }
        if (quest.IsComplete && QuestLog.MyInstance.HasQuest(quest))
        {
            CompleteBtn.SetActive(true);
        }
        else
        {
            CompleteBtn.SetActive(false);
        }

        this.selectedQuest = quest;

        string objectives = string.Empty;
        string title = quest.MyTitle;
        string description = quest.MyDescription;


        foreach (QuestObjective obj in quest.MyCollectObjectives)
        {
            objectives += obj.MyType + ": " + obj.MyAmount + "\n";
        }
        foreach (QuestObjective obj in quest.MyKillObjectives)
        {
            objectives += obj.MyType + ": " + obj.MyAmount + "\n";
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
    public void Accept()
    {
        AudioManager.instance.Play("ButtonClick");
        QuestLog.MyInstance.AcceptQuest(selectedQuest);
        Back();
    }
    public void Back()
    {
        AudioManager.instance.Play("ButtonClick");
        ShowQuests(questGiver);
    }
    public void Exit()
    {
        AudioManager.instance.Play("ButtonClick");
        descriptionWindow.SetActive(false);
        this.gameObject.SetActive(false);
    }



    public void Complete()
    {
        if (selectedQuest.IsComplete)
        {
            for (int i = 0; i < questGiver.MyQuests.Length; i++)
            {
                if(selectedQuest == questGiver.MyQuests[i])
                {
                    questGiver.MyCompletedQuests.Add(selectedQuest.MyTitle);
                    questGiver.MyQuests[i] = null;
                    selectedQuest.MyQuestGiver.UpdateQuestStatus();
                }
            }
            foreach (CollectObjective objective in selectedQuest.MyCollectObjectives)
            {
                //Inventory.instance.onItemChangedCallback -= objective.UpdateItemCount;
                GameManager.instance.onitemchangedEvent -= new OnItemsChanged(objective.UpdateItemCount);
                objective.Complete();
            }
            foreach (KillObjective obje in selectedQuest.MyKillObjectives)
            {
                GameManager.instance.killConfirmedEvent -= new KillConfirmed(obje.UpdateKillCount);
            }
            QuestLog.MyInstance.RemoveQuest(selectedQuest.MyQuestScript);
            AudioManager.instance.Play("CompleteQuest");
            Back();
            Rewards();
        }
    }

    public void Rewards()
    {
        if(selectedQuest.ExpReward > 0)
        {
            CharacterStats.instance.AddExp(selectedQuest.ExpReward);
        }
        if (selectedQuest.MoneyReward != null)
        {
            Inventory.instance.AddCoins(selectedQuest.MoneyReward, selectedQuest.MoneyRewardAmount);
        }
        if(selectedQuest.itemReward != null)
        {
            Inventory.instance.Add(selectedQuest.itemReward, selectedQuest.ItemRewardAmount);
        }
        
    }
    
}
