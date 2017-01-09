using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerInventoryScene : MonoBehaviour
{
    public void LoadLobbyScene()
    {
        Inventory.Instance.WriteInFile();
        SceneManager.LoadScene("Lobby");
    }

    public void AddItem()
    {
        Inventory.Instance.AddItem(Random.Range(0,2));
    }
}
