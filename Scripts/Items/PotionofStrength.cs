using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Potions/StrengthPotions")]

public class PotionofStrength : Item, ItemDescription
{

    public int BonusStrength;

    public override void Use()
    {
        if (!CharacterStats.instance.isPotion)
        {
            AudioManager.instance.Play("PotionUse");
            CharacterStats.instance.IncreaseStrength(BonusStrength);
            Inventory.instance.Remove(this, 1);
        }

    }

    public override string GetDescription()
    {
        return "<color=#0B9819>" + name + "</color=#0B9819>"
            + "\n " + Description + " " + "<color=#0B9819>" + BonusStrength + "</color=#0B9819>" + " for 3 minutes";
    }

}