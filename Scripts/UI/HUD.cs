using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    public Slider hpslider;
    public Slider manaslider;
    public Slider expslider;


    public void SetMaxHealth(float health)
    {
        hpslider.maxValue = health;
        hpslider.value = health;
    }

    public void SetHealth(float health)
    {
        hpslider.value = health;
    }


    public void SetMaxMana(float mana)
    {
        manaslider.maxValue = mana;
        manaslider.value = mana;
    }

    public void SetMana(float mana)
    {
        manaslider.value = mana;
    }


    public void SetMaxExp(float exp)
    {
        expslider.maxValue = exp;
        expslider.value = exp;
    }

    public void SetExp(float exp)
    {
        expslider.value = exp;
    }
}
