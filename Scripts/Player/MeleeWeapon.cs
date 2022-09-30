using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{

    public float maxDistance;

    private float nextAttackTime = 0f;
    public float attackRate = 2f;
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, -transform.forward * maxDistance);
    }
    public void Attack()
    {
        RaycastHit hit;
        if (Time.time >= nextAttackTime)
        {
            if (Physics.Raycast(transform.position, -transform.forward, out hit, maxDistance) && (hit.collider.CompareTag("EnemyMelee") || hit.collider.CompareTag("EnemyRanged")))
            {
                EnemyStats enemystats = hit.collider.gameObject.GetComponent<EnemyStats>();
                enemystats.TakeDamage(CharacterStats.instance.Strength.GetValue());
            }
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }
    public void StrongAttack()
    {
        RaycastHit hit;
        if (Time.time >= nextAttackTime)
        {
            if (Physics.Raycast(transform.position, -transform.forward, out hit, maxDistance) && (hit.collider.CompareTag("EnemyMelee") || hit.collider.CompareTag("EnemyRanged")))
            {
                EnemyStats enemystats = hit.collider.gameObject.GetComponent<EnemyStats>();
                enemystats.TakeDamage(CharacterStats.instance.Strength.GetValue()*1.5f);
            }
            nextAttackTime = Time.time + 1f / attackRate*2.5f;
        }
    }
}
