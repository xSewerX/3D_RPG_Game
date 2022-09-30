using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void KillConfirmed(EnemyStats enemyStats);
public delegate void OnItemsChanged(Item item);

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    private OptionsMenu options;
    

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        options.SetVolume(-30);
    }
    

    public event KillConfirmed killConfirmedEvent;

    public void OnKillConfirmed(EnemyStats enemyStats)
    {
        if (killConfirmedEvent != null)
        {
            killConfirmedEvent(enemyStats);
        }
    }

    public event OnItemsChanged onitemchangedEvent;

    public void OnItemsChanged(Item item)
    {
        if(onitemchangedEvent != null)
        {
            onitemchangedEvent(item);
        }
    }
    
}
