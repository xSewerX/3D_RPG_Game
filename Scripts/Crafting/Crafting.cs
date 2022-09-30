using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    Recipe recipe;

    private void Start()
    {
        recipe = GetComponentInParent<Recipe>();
    }

    public void Craft()
    {
        
    }
}
