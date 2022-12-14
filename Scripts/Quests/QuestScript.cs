using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestScript : MonoBehaviour
{
    public Quest MyQuest { get; set; }
    private bool markedComplete = false;

    public void Select()
    {
        GetComponent<TextMeshProUGUI>().color = Color.yellow;
        QuestLog.MyInstance.ShowDescription(MyQuest);
    }

    public void Deselect()
    {
        GetComponent<TextMeshProUGUI>().color = Color.white;
    }

    public void IsComplete()
    {
        if (MyQuest.IsComplete && !markedComplete)
        {
            markedComplete = true;
            GetComponent<TextMeshProUGUI>().text += " [C]";
            QuestMessage.instance.WriteMessage(string.Format("{0} (Complete)", MyQuest.MyTitle));
        }
        else if (!MyQuest.IsComplete)
        {
            markedComplete = false;
            GetComponent<TextMeshProUGUI>().text = MyQuest.MyTitle;
        }
    }
}
