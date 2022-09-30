using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    private EnemyStats enemystats;
    private bool cooldown = false;
    private void Start()
    {
        enemystats = GetComponentInParent<EnemyStats>();    
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (cooldown == false)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                CharacterStats.instance.TakeDamage(enemystats.Damage);
            }
            cooldown = true;
            Invoke("ResetCD", 1.5f);
        }
    }
    private void ResetCD()
    {
        cooldown = false;
    }
}
