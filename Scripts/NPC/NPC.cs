using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using System;

public class NPC : MonoBehaviour
{
    [HideInInspector]
    public Transform player;
    public Transform minimapIcon;
    [HideInInspector]
    public bool playernear;
    [HideInInspector]
    public InputAction interaction;
    public @StarterAsset playerControls;
    public GameObject NPCUI;
    public GameObject infoimage;
    public GameObject Inventory;

    private void Awake()
    {
        playerControls = new @StarterAsset();
        player = GameObject.FindGameObjectWithTag("PlayerHead").transform;
    }
    private void Start()
    {
        
    }
    private void Update()
    {

        if (playernear)
        {
            Vector3 lookAtPlayer = player.position;
            lookAtPlayer.y = transform.position.y;
            transform.LookAt(lookAtPlayer);
           
        }
    }
    private void LateUpdate()
    {
        minimapIcon.transform.rotation = Quaternion.Euler(90f, 90f, -90f + gameObject.transform.rotation.z * -1.0f);
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

    public virtual void Interaction(InputAction.CallbackContext obj)
    {
        if (playernear)
        {
            NPCUI.SetActive(!NPCUI.activeSelf);
            Inventory.SetActive(!Inventory.activeSelf);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnTriggerEnter(Collider collder)
    {
        if (collder.gameObject.CompareTag("Player"))
        {
            playernear = true;
            infoimage.SetActive(true);
            
        }

    }
    public virtual void OnTriggerExit(Collider collder)
    {
        if (collder.gameObject.CompareTag("Player"))
        {
            playernear = false;
            NPCUI.SetActive(false);
            infoimage.SetActive(false);
            Inventory.SetActive(false);


            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }
}
