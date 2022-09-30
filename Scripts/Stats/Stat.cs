using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]
    public float baseValue;
    public float finalValue;

   

    private List<float> modifiers = new List<float>();


    public float GetValue()
    {
        finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void AddModifier(float modifier)
    {
        if(modifier != 0)
        {
            modifiers.Add(modifier);
            
        }
    }

    public void RemoveModifier(float modifier)
    {
        if (modifier != 0)
        {
            modifiers.Remove(modifier);
            
        }
    }

    public void AddStat()
    {
        baseValue++;
    }
    public void RemoveStat()
    {
        baseValue--;
    }

    public void AddSpeed()
    {
        baseValue += 0.1f;
    }
    public void RemoveSpeed()
    {
        baseValue -= 0.1f;
    }
}
