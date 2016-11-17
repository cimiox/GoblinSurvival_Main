using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance = null;

    public int CountEnemyDeath { get; set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != null)
            Destroy(this);

        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (ConnectionLobby.Instance == null)
            return;

        if (CountEnemyDeath == ConnectionLobby.Instance.countEnemy)
        {
            //Inventory.AddItem(Resources.Load("Inventory/Strength") as GameObject);
        }
    }
}
