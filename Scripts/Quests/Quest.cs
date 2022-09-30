using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField]
    private string title;
    [SerializeField]
    private string description;

    [Header("Rewards")]
    public int ExpReward;
    public Item MoneyReward;
    public int MoneyRewardAmount;
    public Item itemReward;
    public int ItemRewardAmount;


    [SerializeField]
    private CollectObjective[] collectObjectives;
    [SerializeField]
    private KillObjective[] killObjectives;

    public QuestScript MyQuestScript { get; set; }
    public QuestGiver MyQuestGiver { get; set; }

    public string MyTitle
    {
        get
        {
            return title;
        }
        set
        {
            title = value;
        }
    }

    public string MyDescription
    {
        get
        {
            return description;
        }
        set
        {
            description = value;
        }
    }
    public CollectObjective[] MyCollectObjectives
    {
        get
        {
            return collectObjectives;
        }
    }

    public bool IsComplete
    {
        get
        {
            foreach (QuestObjective obj in collectObjectives)
            {
                if (!obj.IsComplete)
                {
                    return false;
                }
            }
            foreach (QuestObjective obj in killObjectives)
            {
                if (!obj.IsComplete)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public KillObjective[] MyKillObjectives { get => killObjectives; set => killObjectives = value; }
}
