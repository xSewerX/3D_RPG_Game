using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField]
    private Quest[] quests;

    [SerializeField]
    private Sprite question, questionGrey, exclamation, minimap_question, minimap_questionGrey, minimap_exclamation;
    [SerializeField]
    private SpriteRenderer statusRenderer, minimapRenderer;
    [SerializeField]
    private int QuestGiverID;

    private List<string> completedQuests = new List<string>();

    public Quest[] MyQuests
    {
        get
        {
            return quests;
        }
    }

    public int MyQuestGiverID { get => QuestGiverID;}

    public List<string> MyCompletedQuests
    {
        get
        {
            return completedQuests;
        }
        set
        {
            completedQuests = value;
            foreach (string title in completedQuests)
            {
                for (int i = 0; i < quests.Length; i++)
                {
                    if(quests[i] != null && quests[i].MyTitle == title)
                    {
                        quests[i] = null;
                    }
                }
            }
        }
    }

    private void Start()
    {
        if (quests != null)
        {
            foreach (Quest quest in quests)
            {
                if(quest != null)
                {
                    quest.MyQuestGiver = this;
                }
                
            }
        }
        
    }

    public void UpdateQuestStatus()
    {

        int count = 0;
        foreach (Quest quest in quests)
        {
            if (quest != null)
            {
                if (quest.IsComplete && QuestLog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = question;
                    minimapRenderer.sprite = minimap_question;
                    break;
                }
                else if (!QuestLog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = exclamation;
                    minimapRenderer.sprite = minimap_exclamation;
                    break;
                }
                else if (!quest.IsComplete && QuestLog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = questionGrey;
                    minimapRenderer.sprite = minimap_questionGrey;
                }
            }
            else
            {
                count++;

                if(count == quests.Length)
                {
                    statusRenderer.enabled = false;
                    minimapRenderer.enabled = false;
                }
            }
        }
    }

}
