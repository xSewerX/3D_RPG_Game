using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SavedGame : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI dateTime, healthText, ManaText, LevelText;
    [SerializeField]
    private Image health, mana, exp;
    [SerializeField]
    private GameObject visuals;

    public int index;

    

    public void ShowInfo(SaveData saveData)
    {
        visuals.SetActive(true);
        dateTime.text = "Date: " + saveData.MyDateTime.ToString("dd/MM/yyyy") + " - Time: " + saveData.MyDateTime.ToString("H:mm");

        health.fillAmount = saveData.MyPlayerData.MyHP / saveData.MyPlayerData.MyMaxHP.finalValue;
        healthText.text = saveData.MyPlayerData.MyHP + "/" + saveData.MyPlayerData.MyMaxHP.finalValue;

        mana.fillAmount = saveData.MyPlayerData.MyMana / saveData.MyPlayerData.MyMaxMana.finalValue;
        ManaText.text = saveData.MyPlayerData.MyMana + "/" + saveData.MyPlayerData.MyMaxMana.finalValue;

        exp.fillAmount = saveData.MyPlayerData.MyExp / saveData.MyPlayerData.MyMaxExp;
        LevelText.text = "Level " + saveData.MyPlayerData.MyLevel.ToString();
    }

    public void HideVisuals()
    {
        visuals.SetActive(false);
    }
    
}
