using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC_QuestGiver : NPC
{
    public GameObject QuestUI;
    private QuestGiver qg;
    public GameObject ChooseWindow;
    private bool isWindowOpen = false;
    public GameObject ShowNPCui;

    private void Start()
    {
        qg = GetComponent<QuestGiver>();
        
        if(NPCUI == null)
        {
            ShowNPCui.SetActive(false);
        }
    }

    
    public override void Interaction(InputAction.CallbackContext obj)
    {
        if (playernear)
        {
            isWindowOpen = false;
            QuestUI.SetActive(false);
            Inventory.SetActive(false);
            if (NPCUI != null)
            {
                NPCUI.SetActive(false);
            }

            if (isWindowOpen == false)
            {
                ChooseWindow.SetActive(!ChooseWindow.activeSelf);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public override void OnTriggerExit(Collider collder)
    {
        if (collder.gameObject.CompareTag("Player"))
        {
            playernear = false;
            if (NPCUI != null)
            {
                NPCUI.SetActive(false);
            }
            infoimage.SetActive(false);
            Inventory.SetActive(false);
            ChooseWindow.SetActive(false);
            QuestUI.SetActive(false);
            isWindowOpen = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

    public void ShowNPCUI()
    {
        if (NPCUI != null)
        {
            NPCUI.SetActive(!NPCUI.activeSelf);
            ChooseWindow.SetActive(false);
            Inventory.SetActive(!Inventory.activeSelf);
            isWindowOpen = true;
        }
    }

    public void ShowNPCQuestUI()
    {
        if (QuestUI != null)
        {
            QuestUI.SetActive(!QuestUI.activeSelf);
            ChooseWindow.SetActive(false);
            QuestGiverwindow.instance.ShowQuests(qg);
            isWindowOpen = true;
        }
    }
}
