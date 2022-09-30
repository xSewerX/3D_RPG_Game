using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;
using System;

public class CharacterStats : MonoBehaviour
{
    #region Singleton
    public static CharacterStats instance;

    private void Awake()
    {
        instance = this;
        currentHealth = MaxHealth.GetValue();
        currentMana = MaxMana.GetValue();
        UpdateStats();
    }
    #endregion

     
    public TextMeshProUGUI combatTextPrefab, CurrentHealthText, MaxHealthText, CurrentManaText, MaxManaText, ArmorText, StrengthText,
                           DexterityText, PowerText, SpeedText, LevelText, StatPointsText, ExpText;

    public TextMeshProUGUI HpbarText, ManabarText, LevelbarText;
    public Transform LevelUPinfo;
    public GameObject LevelUpInoText, PlayerCombatCanvas;

    public HUD hud;

    public Stat MaxHealth;
    public float currentHealth;

    public Stat MaxMana;
    public float currentMana;

    public Stat MovementSpeed;
    public Stat Armor;
    public Stat Strength;
    public Stat Dexterity;
    public Stat Power;
    [HideInInspector]
    public bool isPotion = false;
    [HideInInspector]
    public float potionDex;
    [HideInInspector]
    public float potionStrength;
    [HideInInspector]
    public float potionPower;
    [HideInInspector]
    public float PotionTime = 180;

    [HideInInspector]
    public float MaxExp = 100;
    private float AddMaxExp;
    [SerializeField]
    private float overflow;
    [HideInInspector]
    public float currentExp;
    [HideInInspector]
    public int Level = 1;
    [HideInInspector]
    public int statPoints = 0;
    [HideInInspector]
    public float LevelPercentage;

    public GameObject GameOverScreen;
    public GameObject PotionBuff;
    public Transform PlayerObject;
    public GameObject RespawnPoint;
    public TextMeshProUGUI PotionTimerTxt;
    [SerializeField]
    private GameObject NPClvl5, NPClvl12;

    Animator anim;
    [HideInInspector]
    public PlayerControls playerControls;

    


    void Start()
    {
        playerControls = GetComponent<PlayerControls>();
        anim = GetComponent<Animator>();
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
        PlayerInCombat(false);
    }
    private void Update()
    {
        if (PotionTime > 0 && isPotion)
        {
            PotionBuff.SetActive(true);
            PotionTime -= Time.deltaTime;
            int PotionTimeRemaining = Mathf.FloorToInt(PotionTime) + 1;
            PotionTimerTxt.text = PotionTimeRemaining.ToString();
        }
        if (PotionTime <= 0 && potionStrength > 0)
        {
            StrengthEnd();
            potionStrength = 0;
            PotionTime = 10;
        }
        if (PotionTime <= 0 && potionDex > 0)
        {
            DexEnd();
            potionDex = 0;
            PotionTime = 10;
        }
        if (PotionTime <= 0 && potionPower > 0)
        {
            PowerEnd();
            potionPower = 0;
            PotionTime = 10;
        }
        if(Level >= 5)
        {
            NPClvl5.SetActive(true);
        }
        if(Level >= 12)
        {
            NPClvl12.SetActive(true);
        }
    }

    public void PlayerInCombat(bool isInCombat)
    {
        if(isInCombat == false)
        {
            InvokeRepeating("HPRegeneration", 10f, 5f);
            InvokeRepeating("ManaRegeneration", 10f, 5f);
        }
        else
        {
            CancelInvoke("HPRegeneration");
            CancelInvoke("ManaRegeneration");
        }
        
    }

    private void HPRegeneration()
    {
        if(currentHealth < MaxHealth.GetValue()*0.5f)
        {
            Heal(2);
        }
        else
        {
            CancelInvoke("HPRegeneration");
        }
    }
    private void ManaRegeneration()
    {
        if (currentMana < MaxMana.GetValue())
        {
            RestoreMana(0.05f);
        }
        else
        {
            CancelInvoke("ManaRegeneration");
        }
    }

    public void TakeDamage(float damage)
    {
        if (anim.GetBool("isDead") == false)
        {
            anim.SetTrigger("GetHit");
            AudioManager.instance.Play("PlayerHit");


            damage -= Armor.GetValue();
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            currentHealth -= damage;

            if(damage > 0)
            {
                combatTextPrefab.text = "-" + damage.ToString();
                combatTextPrefab.color = Color.red;
                Instantiate(combatTextPrefab, PlayerCombatCanvas.transform);
            }
            

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                playerControls.enabled = false;
                Invoke("Die", 2f);
                anim.SetBool("isDead", true);
                AudioManager.instance.Play("PlayerDeath");
            }
            UpdateStats();
        }
    }
    private void Heal(float heal)
    {
        currentHealth += heal;
        
        if (currentHealth > MaxHealth.GetValue())
        {
            currentHealth = MaxHealth.GetValue();
        }
        UpdateStats();
    }

    #region PotionsEffects
    public void PotionHeal(float heal)
    {
        float healValue = MaxHealth.GetValue() * heal;
        currentHealth += healValue;

        combatTextPrefab.text = "+" + healValue.ToString();
        combatTextPrefab.color = Color.green;
        Instantiate(combatTextPrefab, PlayerCombatCanvas.transform);

        if (currentHealth > MaxHealth.GetValue())
        {
            currentHealth = MaxHealth.GetValue();
        }
        UpdateStats();
    }

    public void RestoreMana(float mana)
    {
        currentMana += MaxMana.GetValue()*mana;
        if (currentMana > MaxMana.GetValue())
        {
            currentMana = MaxMana.GetValue();
        }
        UpdateStats();
    }

    public void IncreaseStrength(float strength)
    {
        Strength.baseValue += strength;
        MaxHealth.baseValue += strength;
        potionStrength = strength;
        //Invoke("StrengthEnd", PotionTime);
        isPotion = true;
        PotionBuff.SetActive(true);
        UpdateStats();
    }
    private void StrengthEnd()
    {
        Strength.baseValue -= potionStrength;
        MaxHealth.baseValue -= potionStrength;
        PotionBuff.SetActive(false);
        isPotion = false;
        if (currentHealth > MaxHealth.GetValue())
        {
            currentHealth = MaxHealth.GetValue();
        }
        UpdateStats();
    }
    
    public void IncreaseDexterity(float dex)
    {
        Dexterity.baseValue += dex;
        MovementSpeed.baseValue += dex * 0.1f;
        potionDex = dex;
        //Invoke("DexEnd", PotionTime);
        isPotion = true;
        PotionBuff.SetActive(true);
        UpdateStats();
    }
    private void DexEnd()
    {
        Dexterity.baseValue -= potionDex;
        MovementSpeed.baseValue -= potionDex * 0.1f;
        PotionBuff.SetActive(false);
        UpdateStats();
        isPotion = false;
    }
    public void IncreasePower(float power)
    {
        Power.baseValue += power;
        MaxMana.baseValue += power;
        potionPower = power;
       // Invoke("PowerEnd", PotionTime);
        isPotion = true;
        PotionBuff.SetActive(true);
        UpdateStats();
    }
    private void PowerEnd()
    {
        Power.baseValue -= potionPower;
        MaxMana.baseValue -= potionPower;
        PotionBuff.SetActive(false);
        isPotion = false;
        if (currentMana > MaxMana.GetValue())
        {
            currentMana = MaxMana.GetValue();
        }
        UpdateStats();
    }
    #endregion

    public void AddExp(float exp)
    {
        currentExp += exp;
        if(currentExp > MaxExp)
        {
            overflow = currentExp - MaxExp;
        }

        combatTextPrefab.text = "+" + exp.ToString() + " XP";
        combatTextPrefab.color = Color.magenta;
        Instantiate(combatTextPrefab, PlayerCombatCanvas.transform);

        if (currentExp >= MaxExp)
        {
            LevelUP();
        }
        UpdateStats();
    }

    private void AddOverflowExp(float overflowEXP)
    {
        currentExp += overflowEXP;
        if (currentExp > MaxExp)
        {
            overflow = currentExp - MaxExp;
        }

        if (currentExp >= MaxExp)
        {
            LevelUP();
        }
        UpdateStats();
    }
   private void LevelUP()
    {

        AudioManager.instance.Play("LevelUP");
        Level++;
        statPoints++;
        currentExp = 0;
        AddMaxExp = Level * 12;
        MaxExp = 110 + AddMaxExp;
        if (overflow > 0)
        {
            AddOverflowExp(overflow);
        }
        
        if (currentExp >= MaxExp)
        {
            LevelUP();
        }


        GameObject go = Instantiate(LevelUpInoText, LevelUPinfo);
        go.GetComponent<TextMeshProUGUI>().text = " Level " + Level;
        Destroy(go, 2f);
    }

    public void Die()
    {
        GameOverScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0;

    }
    public void Respawn()
    {
        playerControls.enabled = true;
        GameOverScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        anim.SetBool("isDead", false);
        Heal(MaxHealth.GetValue()*0.5f);
        PlayerObject.transform.position = RespawnPoint.transform.position;
        Physics.SyncTransforms();
    }
    #region StatsUpdate
    public void UpdateStats()
    {
        double ExpPercentage = Math.Round(LevelPercentage = (currentExp / MaxExp) * 100, 2);
        double speed = Math.Round(MovementSpeed.GetValue(), 1);
        double currentManaRound = Math.Round(currentMana, 1);
        double currentHPRound = Math.Round(currentHealth, 1);

        if(currentMana < 0)
        {
            currentMana = 0;
        }

        if (currentHealth > MaxHealth.GetValue())
        {
            currentHealth = MaxHealth.GetValue();
        }
        if(currentMana > MaxMana.GetValue())
        {
            currentMana = MaxMana.GetValue();
        }

        hud.SetMaxHealth(MaxHealth.GetValue());
        hud.SetHealth(currentHealth);

        hud.SetMaxMana(MaxMana.GetValue());
        hud.SetMana(currentMana);

        hud.SetMaxExp(MaxExp);
        hud.SetExp(currentExp);


        HpbarText.text = currentHPRound.ToString() + "/" + MaxHealth.GetValue().ToString();
        ManabarText.text = currentManaRound.ToString() + "/" + MaxMana.GetValue().ToString();
        LevelbarText.text = "Level " + Level.ToString();

        CurrentHealthText.text = "Current: " + currentHPRound.ToString();
        MaxHealthText.text = "Max: " + MaxHealth.GetValue().ToString();
        CurrentManaText.text = "Current: " + currentManaRound.ToString();
        MaxManaText.text = "Max: " + MaxMana.GetValue().ToString();
        ArmorText.text = "Armor: " + Armor.GetValue().ToString();
        StrengthText.text = "Strength: " + Strength.GetValue().ToString();
        DexterityText.text = "Dexterity: " + Dexterity.GetValue().ToString();
        PowerText.text = "Power: " + Power.GetValue().ToString();
        SpeedText.text = "Speed: " + speed.ToString();
        LevelText.text = "Level: " + Level.ToString();
        StatPointsText.text = "Stat Points: " + statPoints.ToString();
        ExpText.text = "EXP: "+ExpPercentage.ToString() + "%";
        
    }

    public void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {


        if (newItem != null)
        {

            Armor.AddModifier(newItem.armor);
            Strength.AddModifier(newItem.strengthModifier);
            Dexterity.AddModifier(newItem.dexterityModifier);
            Power.AddModifier(newItem.powerModifier);
            MaxHealth.AddModifier(newItem.strengthModifier);
            MaxMana.AddModifier(newItem.powerModifier);
            MovementSpeed.AddModifier(newItem.dexterityModifier * 0.1f);
            UpdateStats();

        }
        if (oldItem != null)
        {


            Armor.RemoveModifier(oldItem.armor);
            Strength.RemoveModifier(oldItem.strengthModifier);
            Dexterity.RemoveModifier(oldItem.dexterityModifier);
            Power.RemoveModifier(oldItem.powerModifier);
            MaxHealth.RemoveModifier(oldItem.strengthModifier);
            MaxMana.RemoveModifier(oldItem.powerModifier);
            MovementSpeed.RemoveModifier(oldItem.dexterityModifier * 0.1f);
            UpdateStats();
            
        }
    }
    #endregion
}
