using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class Gathering : MonoBehaviour
{
    PlayerControls playermovement;
    private InputAction interaction;
    public @StarterAsset playerControls;
    bool playerNear;
    Animator anim;
    private GameObject pickaxe;
    public Item item;
    public GameObject infoimage;
    public int amount;
    public int respawnTime;
    
    private bool cooldown = false;



    private void Awake()
    {
        playerControls = new @StarterAsset();
        pickaxe = GameObject.FindGameObjectWithTag("Pickaxe");
        playermovement = FindObjectOfType<PlayerControls>();
        
    }

    private void OnEnable()
    {
        interaction = playerControls.Player.Interaction;
        interaction.Enable();
        interaction.performed += Interaction;
    }

    private void OnDisable()
    {
        interaction.Disable();
    }

    private void Interaction(InputAction.CallbackContext obj)
    {
        if(cooldown == false)
        {
            if (playerNear == true)
            {
                playermovement.isGathering = true;
                if (gameObject.CompareTag("Ore"))
                {
                    pickaxe.GetComponent<MeshRenderer>().enabled = true;
                    if (EquipmentManager.instance.Weapon.enabled == true)
                    {
                        EquipmentManager.instance.weaponobject.SetActive(false);
                        EquipmentManager.instance.isWeaponEquipped = false;
                    }
                    AudioManager.instance.Play("Mining");
                    anim.SetTrigger("Mining");
                    Invoke("EndGathering", 2.61f);
                }
                if (gameObject.CompareTag("Gatherable"))
                {
                    if (EquipmentManager.instance.Weapon.enabled == true)
                    {
                        EquipmentManager.instance.weaponobject.SetActive(false);
                        EquipmentManager.instance.isWeaponEquipped = false;
                    }
                    anim.SetTrigger("Gathering");
                    Invoke("EndGathering", 2f);
                }
            }
            Invoke("ResetCooldown", 3f);
            cooldown = true;
        }
    }
    
    private void ResetCooldown()
    {
        cooldown = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            anim = collider.GetComponent<Animator>();
            playerNear = true;
            infoimage.SetActive(true);

        }
        
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerNear = false;
            infoimage.SetActive(false);
        }

    }
    void EndGathering()
    {
        playermovement.isGathering = false;
        Inventory.instance.Add(item, amount);
        playerNear = false;
        infoimage.SetActive(false);
        gameObject.SetActive(false);
        pickaxe.GetComponent<MeshRenderer>().enabled = false;
        if (EquipmentManager.instance.Weapon.enabled == true)
        {
            EquipmentManager.instance.isWeaponEquipped = true;
            EquipmentManager.instance.weaponobject.SetActive(true);
        }
        Invoke("Respawn",respawnTime);
    }
    void Respawn()
    {
        gameObject.SetActive(true);
    }
    
}


