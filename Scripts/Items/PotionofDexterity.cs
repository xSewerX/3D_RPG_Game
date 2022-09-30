using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Potions/DexPotions")]

public class PotionofDexterity : Item, ItemDescription
{

    public int BonusDexterity;

    public override void Use()
    {
        if (!CharacterStats.instance.isPotion)
        {
            AudioManager.instance.Play("PotionUse");
            CharacterStats.instance.IncreaseDexterity(BonusDexterity);
            Inventory.instance.Remove(this, 1);
        }
        
    }

    public override string GetDescription()
    {
        return "<color=#0B9819>" + name + "</color=#0B9819>"
            + "\n " + Description + " " + "<color=#0B9819>" + BonusDexterity + "</color=#0B9819>" + " for 3 minutes";
    }
}