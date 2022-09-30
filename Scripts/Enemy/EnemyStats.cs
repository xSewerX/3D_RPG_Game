using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyStats : MonoBehaviour
{
    [SerializeField]
    private string Type;

    public Item item;
    public Item coins;
    public int coinsAmount;
    private Slider hpslider;
    private AIController aIController;

    public float MaxHealth = 30;
    private float currentHealth;
    public int ExpPoints;
    public int Damage = 5;
    public int Armor = 2;
    public int TimeToRespawn = 5;
    public GameObject EnemyToRespawn;
    [SerializeField]
    private TextMeshProUGUI combatTextPrefab;
    [SerializeField]
    private GameObject EnemyCanvas;

    public Transform RespawnPoint, minimapIcon;

    private int randomloot;
    Animator anim;
    private void Awake()
    {

        anim = GetComponentInChildren<Animator>();
        hpslider = GetComponentInChildren<Slider>();
        aIController = GetComponent<AIController>();
        currentHealth = MaxHealth;
        
        aIController.enabled = true;
    }
    private void Start()
    {
        hpslider.maxValue = MaxHealth;
        hpslider.value = currentHealth;
    }

    private void LateUpdate()
    {
        minimapIcon.transform.rotation = Quaternion.Euler(90f, 90f, 180f + gameObject.transform.rotation.z * -1.0f);
    }
    public void TakeDamage(float damage)
    {
        if (anim.GetBool("isDead") == false)
        {
            
            anim.SetTrigger("GetHit");
            damage -= Armor;
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            combatTextPrefab.text = damage.ToString();
            combatTextPrefab.color = Color.yellow;
            Instantiate(combatTextPrefab, EnemyCanvas.transform);
            
            

            currentHealth -= damage;
            hpslider.value = currentHealth;
            if (currentHealth <= 0)
            {
                Die();
            }
        }
        
    }

    public void Die()
    {
        AudioManager.instance.Play("EnemyDeath");
        aIController.enabled = false;
        Invoke("Death", 5f);
        anim.SetBool("isDead", true);
        GameManager.instance.OnKillConfirmed(this);
        CharacterStats.instance.AddExp(ExpPoints);
        Inventory.instance.AddCoins(coins, coinsAmount);
        CharacterStats.instance.PlayerInCombat(false);
        RandomLoot();
    }
    private void Death()
    {
        gameObject.SetActive(false);
        Invoke("Respawn", TimeToRespawn);
    }

    void Respawn()
    {
        anim.SetBool("isDead", false);
        GameObject NewEnemy = Instantiate(EnemyToRespawn);
        NewEnemy.transform.position = RespawnPoint.transform.position;
        NewEnemy.name = name;
        NewEnemy.SetActive(true);
        Destroy(gameObject);
    }
    public void RandomLoot()
    {
        randomloot = Random.Range(0, 101);

        if (randomloot <= 30)
        {
           // Debug.Log(randomloot + " Brak lootu");
        }
        if(randomloot >=31 && randomloot <= 70)
        {
            Inventory.instance.Add(item, 1);
            //Debug.Log(randomloot + " loot 1");
        }
        if (randomloot >= 71 && randomloot <= 90)
        {
            Inventory.instance.Add(item, 2);
           // Debug.Log(randomloot + " loot 2");
        }
        if (randomloot > 90)
        {
            Inventory.instance.Add(item, 3);
           // Debug.Log(randomloot + " loot 3");
        }

    }

    public string MyType
    {
        get
        {
            return Type;
        }
    }
}
