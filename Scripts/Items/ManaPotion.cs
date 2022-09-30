using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Potions/ManaPotions")]

public class ManaPotion : Item, ItemDescription
{
    [Range(0f, 1f)]
    public float RestoreManaPercent;


    public override void Use()
    {
        if (CharacterStats.instance.currentMana != CharacterStats.instance.MaxMana.GetValue())
        {
            AudioManager.instance.Play("PotionUse");
            CharacterStats.instance.RestoreMana(RestoreManaPercent);
            Inventory.instance.Remove(this, 1);
        }
    }

    public override string GetDescription()
    {
        return "<color=#0B9819>" + name + "</color=#0B9819>"
            + "\n " + Description + " " + "<color=#0B9819>" + RestoreManaPercent * 100 + "%" + "</color=#0B9819>" + " mana.";
    }

}