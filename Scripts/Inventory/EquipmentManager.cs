using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton
    public static EquipmentManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public Equipment[] currentEquipment;
    Inventory inventory;

    
    public Image Head, Chest, Legs, Feet, Weapon, Neck, Finger, Amulet;
    
    public Button unequipHead, unequipChest, unequipLegs, unequipFeet, unequipWeapon, unequipNeck, unequipFinger, unequipAmulet;
    
    public GameObject hair, helmet, chest, gloves, shoulders, pants, boots, starterBoots, starterChest, starterPants, weaponobject;


    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;
    [HideInInspector]
    public bool isWeaponEquipped;
    [HideInInspector]
    public int slotindex;

    public void Start()
    {
        inventory = Inventory.instance;

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
        
    }
    private void Update()
    {
        if (isWeaponEquipped)
        {
            weaponobject.SetActive(true);
        }
    }
    public void Equip(Equipment newItem)
    {
         slotindex = (int)newItem.equipmentslot;

        Equipment oldItem = null;

        switch (newItem.equipmentslot)
        {
            case EquipmentSlot.Head:
                Head.sprite = newItem.icon;
                Head.enabled = true;
                unequipHead.interactable = true;
                hair.SetActive(false);
                helmet.SetActive(true);
                break;
            case EquipmentSlot.Chest:
                Chest.sprite = newItem.icon;
                Chest.enabled = true;
                unequipChest.interactable = true;
                chest.SetActive(true);
                shoulders.SetActive(true);
                gloves.SetActive(true);
                starterChest.SetActive(false);
                break;
            case EquipmentSlot.Legs:
                Legs.sprite = newItem.icon;
                Legs.enabled = true;
                unequipLegs.interactable = true;
                pants.SetActive(true);
                starterPants.SetActive(false);
                break;
            case EquipmentSlot.Feet:
                Feet.sprite = newItem.icon;
                Feet.enabled = true;
                unequipFeet.interactable = true;
                boots.SetActive(true);
                starterBoots.SetActive(false);
                break;
            case EquipmentSlot.Neck:
                Neck.sprite = newItem.icon;
                Neck.enabled = true;
                unequipNeck.interactable = true;
                break;
            case EquipmentSlot.Finger:
                Finger.sprite = newItem.icon;
                Finger.enabled = true;
                unequipFinger.interactable = true;
                break;
            case EquipmentSlot.Weapon:
                Weapon.sprite = newItem.icon;
                Weapon.enabled = true;
                unequipWeapon.interactable = true;
                weaponobject.SetActive(true);
                isWeaponEquipped = true;
                break;
            case EquipmentSlot.Amulet:
                Amulet.sprite = newItem.icon;
                Amulet.enabled = true;
                unequipAmulet.interactable = true;
                break;

        }
       


        if (currentEquipment[slotindex] != null)
        {
            oldItem = currentEquipment[slotindex];
            inventory.Add(oldItem,1);
        }
        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }
        
        currentEquipment[slotindex] = newItem;
    }

    public void Unequip(int slotindex)
    {
        AudioManager.instance.Play("ButtonClick");
        if (currentEquipment[slotindex] != null)
        {
            Equipment oldItem = currentEquipment[slotindex];
            inventory.Add(oldItem,1);



            switch (oldItem.equipmentslot)
            {
                case EquipmentSlot.Head:
                    Head.sprite = oldItem.icon;
                    Head.enabled = false;
                    unequipHead.interactable = false;
                    hair.SetActive(true);
                    helmet.SetActive(false);
                    break;
                case EquipmentSlot.Chest:
                    Chest.sprite = oldItem.icon;
                    Chest.enabled = false;
                    unequipChest.interactable = false;
                    chest.SetActive(false);
                    shoulders.SetActive(false);
                    gloves.SetActive(false);
                    starterChest.SetActive(true);
                    break;
                case EquipmentSlot.Legs:
                    Legs.sprite = oldItem.icon;
                    Legs.enabled = false;
                    unequipLegs.interactable = false;
                    pants.SetActive(false);
                    starterPants.SetActive(true);
                    break;
                case EquipmentSlot.Feet:
                    Feet.sprite = oldItem.icon;
                    Feet.enabled = false;
                    unequipFeet.interactable = false;
                    boots.SetActive(false);
                    starterBoots.SetActive(true);
                    break;
                case EquipmentSlot.Neck:
                    Neck.sprite = oldItem.icon;
                    Neck.enabled = false;
                    unequipNeck.interactable = false;
                    break;
                case EquipmentSlot.Finger:
                    Finger.sprite = oldItem.icon;
                    Finger.enabled = false;
                    unequipFinger.interactable = false;
                    break;
                case EquipmentSlot.Weapon:
                    Weapon.sprite = oldItem.icon;
                    Weapon.enabled = false;
                    unequipWeapon.interactable = false;
                    weaponobject.SetActive(false);
                    isWeaponEquipped = false;
                    break;
                case EquipmentSlot.Amulet:
                    Amulet.sprite = oldItem.icon;
                    Amulet.enabled = false;
                    unequipAmulet.interactable = false;
                    break;
            }
            
            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }

            currentEquipment[slotindex] = null;
        }
        
    }
    
}
