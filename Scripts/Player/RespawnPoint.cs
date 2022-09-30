using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using TMPro;

public class RespawnPoint : MonoBehaviour
{
    PlayerControls playermovement;
    private InputAction interaction;
    public @StarterAsset playerControls;
    bool playerNear;
    public GameObject infoimage, chatInfo;
    [SerializeField]
    private Transform Chat;
    
    private void Awake()
    {
        playerControls = new @StarterAsset();
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
        if (playerNear == true)
        {
            SetRespawn();
        }
    }

    private void SetRespawn()
    {
        CharacterStats.instance.RespawnPoint = gameObject;

        GameObject go = Instantiate(chatInfo, Chat);
        TextMeshProUGUI textObject = go.GetComponent<TextMeshProUGUI>();
        textObject.color = Color.white;
        textObject.text = "Spawn point has been set to: " + this.name;
        
        go.transform.SetAsFirstSibling();

        Destroy(go, 3f);
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
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
    
}

