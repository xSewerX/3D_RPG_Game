using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName =  "Inventory/Equipment")]




public class Equipment : Item, ItemDescription
{
    public EquipmentSlot equipmentslot;

    
    public int armor;

    public int strengthModifier;
    public int dexterityModifier;
    public int powerModifier;
    
    public override void Use()
    {
        base.Use();
        AudioManager.instance.Play("Equip");
        EquipmentManager.instance.Equip(this);
        RemoveFromInventory();

    }
    

    public override string GetDescription()
    {
        switch (equipmentslot)
        {
            case EquipmentSlot.Weapon:
                return "<color=#CBC61A>" + name + "</color=#CBC61A>"
            + "\n Strength Modifier: " + strengthModifier;
            case EquipmentSlot.Neck:
                return "<color=#257CBF>" + name + "</color=#257CBF>" +
            "\n Bonus strength: " + strengthModifier +
            "\n Bonus dexterity: " + dexterityModifier +
            "\n Bonus Power: " + powerModifier;
            case EquipmentSlot.Finger:
                return "<color=#257CBF>" + name + "</color=#257CBF>" +
            "\n Bonus strength: " + strengthModifier +
            "\n Bonus dexterity: " + dexterityModifier +
            "\n Bonus Power: " + powerModifier;
            case EquipmentSlot.Amulet:
                return "<color=#257CBF>" + name + "</color=#257CBF>" +
            "\n Bonus strength: " + strengthModifier +
            "\n Bonus dexterity: " + dexterityModifier +
            "\n Bonus Power: " + powerModifier;
            default:
                return "<color=#CB6E1E>" + name + "</color=#CB6E1E>"
            + "\n Armor: " + armor +
            "\n Bonus strength: " + strengthModifier +
            "\n Bonus dexterity: " + dexterityModifier +
            "\n Bonus Power: " + powerModifier;

        }


        
    }

}

public enum EquipmentSlot { Head, Chest, Legs, Feet, Weapon, Neck, Finger, Amulet }



