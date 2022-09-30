using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseStat : MonoBehaviour
{
    private CharacterStats characterStats;

    private void Awake()
    {
        characterStats = FindObjectOfType<CharacterStats>();
    }


    public void IncreaseStrengthButton()
    {
        AudioManager.instance.Play("ButtonClick");
        if (characterStats.statPoints > 0)
        {
            characterStats.Strength.AddStat();
            characterStats.MaxHealth.AddStat();
            characterStats.statPoints--;
            characterStats.UpdateStats();
        }
    }
    
    public void DecreaseStrengthButton()
    {
        AudioManager.instance.Play("ButtonClick");
        if (characterStats.Strength.baseValue > 0)
        {
            characterStats.Strength.RemoveStat();
            characterStats.MaxHealth.RemoveStat();
            characterStats.statPoints++;
            characterStats.UpdateStats();
        }
    }

    public void IncreaseDexterityButton()
    {
        AudioManager.instance.Play("ButtonClick");
        if (characterStats.statPoints > 0)
        {
            characterStats.Dexterity.AddStat();
            characterStats.MovementSpeed.AddSpeed();
            characterStats.statPoints--;
            characterStats.UpdateStats();
        }
    }

    public void DecreaseDexterityButton()
    {
        AudioManager.instance.Play("ButtonClick");
        if (characterStats.Dexterity.baseValue > 0)
        {
            characterStats.Dexterity.RemoveStat();
            characterStats.MovementSpeed.RemoveSpeed();
            characterStats.statPoints++;
            characterStats.UpdateStats();
        }
    }

    public void IncreasePowerButton()
    {
        AudioManager.instance.Play("ButtonClick");
        if (characterStats.statPoints > 0)
        {
            characterStats.Power.AddStat();
            characterStats.MaxMana.AddStat();
            characterStats.statPoints--;
            characterStats.UpdateStats();
        }
    }

    public void DecreasePowerButton()
    {
        AudioManager.instance.Play("ButtonClick");
        if (characterStats.Power.baseValue > 0)
        {
            characterStats.Power.RemoveStat();
            characterStats.MaxMana.RemoveStat();
            characterStats.statPoints++;
            characterStats.UpdateStats();
        }
    }

}
