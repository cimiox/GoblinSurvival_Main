using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerInventoryScene : MonoBehaviour
{
    public void LoadLobbyScene()
    {
        Inventory.Instance.WriteInFile();
        SceneManager.LoadScene("Lobby");
    }
}
