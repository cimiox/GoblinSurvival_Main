using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    public GameObject gameManager;
    void Awake()
    {
        if (Manager.Instance == null)
        {
            Instantiate(gameManager);
        }
    }
}
