using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Potions/PowerPotions")]

public class PotionofPower : Item, ItemDescription
{

    public int BonusPower;

    public override void Use()
    {
        if (!CharacterStats.instance.isPotion)
        {
            AudioManager.instance.Play("PotionUse");
            CharacterStats.instance.IncreasePower(BonusPower);
            Inventory.instance.Remove(this, 1);
        }

    }

    public override string GetDescription()
    {
        return "<color=#0B9819>" + name + "</color=#0B9819>"
            + "\n " + Description + " " + "<color=#0B9819>" + BonusPower + "</color=#0B9819>" + " for 3 minutes";
    }

}