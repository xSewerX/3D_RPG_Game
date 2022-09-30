using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    public int damage;

    private void Start()
    {
        StartCoroutine("Destroy");
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            CharacterStats.instance.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
    IEnumerator Destroy()
    {
        yield return  new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
