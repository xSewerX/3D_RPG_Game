using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestScriptNPC : MonoBehaviour
{
    public Quest MyQuest { get; set; }
    

    public void Select()
    {
        QuestGiverwindow.instance.ShowDescription(MyQuest);
    }

    public void Deselect()
    {
    }
}
