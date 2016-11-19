using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject inventoryObj;
    void Awake()
    {
        if (Manager.Instance == null)
        {
            Instantiate(gameManager);
        }

        if (Inventory.Instance == null)
        {
            GameObject go = Instantiate(inventoryObj);
            go.name = "Inventory";
        }
    }
}
