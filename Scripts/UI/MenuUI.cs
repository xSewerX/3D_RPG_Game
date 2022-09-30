using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public GameObject Options, Map, CharacterProfile, Inventory, StatsProfile, Blacksmith,
        Alchemist, Vendor, Jeweler, QuestNPC;
    
    [SerializeField]
    private CanvasGroup CanvasGroupQL, CanvasGroupMenu, CanvasGroupMenuUI, CanvasGroupSaveAndLoad;

    public void GoToStatsPanel()
    {
        AudioManager.instance.Play("ButtonClick");
        CharacterProfile.SetActive(false);
        StatsProfile.SetActive(true);
    }

    public void GoToCharacterProfile()
    {
        AudioManager.instance.Play("ButtonClick");
        CharacterProfile.SetActive(true);
        StatsProfile.SetActive(false);
    }

    public void CharacterProfileMenu()
    {
        AudioManager.instance.Play("ButtonClick");
        CharacterProfile.SetActive(!CharacterProfile.activeSelf);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void InventoryExit()
    {
        AudioManager.instance.Play("ButtonClick");
        Inventory.SetActive(!Inventory.activeSelf);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ExitBlacksmith()
    {
        AudioManager.instance.Play("ButtonClick");
        Blacksmith.SetActive(false);
    }
    public void ExitAlchemist()
    {
        AudioManager.instance.Play("ButtonClick");
        Alchemist.SetActive(false);
    }
    public void ExitVendor()
    {
        AudioManager.instance.Play("ButtonClick");
        Vendor.SetActive(false);
    }
    public void ExitJeweler()
    {
        AudioManager.instance.Play("ButtonClick");
        Jeweler.SetActive(false);
    }
    private void QuestGiverExit()
    {
        AudioManager.instance.Play("ButtonClick");
        QuestNPC.SetActive(false);
    }
    public void MapButton()
    {
        AudioManager.instance.Play("ButtonClick");
        Map.SetActive(!Map.activeSelf);
    }
    public void Questlog()
    {
        AudioManager.instance.Play("ButtonClick");
        if (CanvasGroupQL.alpha == 1)
        {
            QuestLogClose();
        }
        else
        {
            CanvasGroupQL.alpha = 1;
            CanvasGroupQL.blocksRaycasts = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void QuestLogClose()
    {
        AudioManager.instance.Play("ButtonClick");
        CanvasGroupQL.alpha = 0;
        CanvasGroupQL.blocksRaycasts = false;
    }
    public void OpenMenu()
    {
        AudioManager.instance.Play("ButtonClick");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CharacterStats.instance.playerControls.enabled = false;
        Time.timeScale = 0;

        CanvasGroupMenu.alpha = 1;
        CanvasGroupMenu.blocksRaycasts = true;
        

        CharacterProfile.SetActive(false);
        StatsProfile.SetActive(false);
        Inventory.SetActive(false);
        Alchemist.SetActive(false);
        Blacksmith.SetActive(false);
        Vendor.SetActive(false);
        Jeweler.SetActive(false);
        QuestNPC.SetActive(false);
        Map.SetActive(false);

        CanvasGroupQL.alpha = 0;
        CanvasGroupQL.blocksRaycasts = false;

    }
    public void Resume()
    {
        AudioManager.instance.Play("ButtonClick");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        CharacterStats.instance.playerControls.enabled = true;

        CanvasGroupMenu.alpha = 0;
        CanvasGroupMenu.blocksRaycasts = false;
    }
    public void OptionsMenu()
    {
        AudioManager.instance.Play("ButtonClick");
        HideMenu();
        Options.SetActive(true);
    }
    public void SaveAndLoad()
    {
        AudioManager.instance.Play("ButtonClick");
        CanvasGroupSaveAndLoad.alpha = 1;
        CanvasGroupSaveAndLoad.blocksRaycasts = true;
        HideMenu();
    }
    public void Back()
    {
        AudioManager.instance.Play("ButtonClick");
        CanvasGroupSaveAndLoad.alpha = 0;
        CanvasGroupSaveAndLoad.blocksRaycasts = false;
        Options.SetActive(false);
        HideMenu();
    }
    private void HideMenu()
    {
        AudioManager.instance.Play("ButtonClick");
        CanvasGroupMenuUI.alpha = 1;
        CanvasGroupMenuUI.blocksRaycasts = true;
    }

    public void Quit()
    {
        AudioManager.instance.Play("ButtonClick");
        Application.Quit();
        Debug.Log("Quit Game");
    }

}
