using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Potions/HpPotions")]

public class HealthPotion : Item, ItemDescription
{
    [Range(0f, 1f)]
    public float RestoreHealthPercent;
    

    public override void Use()
    {
        if (CharacterStats.instance.currentHealth != CharacterStats.instance.MaxHealth.GetValue())
        {
            AudioManager.instance.Play("PotionUse");
            CharacterStats.instance.PotionHeal(RestoreHealthPercent);
            Inventory.instance.Remove(this, 1);
        }
    }

    public override string GetDescription()
    {
        return "<color=#0B9819>" + name + "</color=#0B9819>"
            + "\n " + Description + " " + "<color=#0B9819>"+ RestoreHealthPercent *100 +"%" + "</color=#0B9819>"+ " health.";
    }

}
