using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public InputField NicknameField;
    public InputField RoomNameField;
    public Button CreateRoom;
    public Button JoinRoom;
    public Button JoinRandomRoom;
    public Button TryAgainButton;
    public Button ShowPlayersButton;
    public Button ShowMenu;
    public Button NextPlayerButton;
    public Button InventoryButton;
    public Text ConnectionProblemInfo;
    public GameObject InventoryPanel;
    public GameObject GroupPanelLobby;
    public GameObject GroupPanelNoConnection;
    public GameObject GoblinStalker;
    public GameObject GoblinSniper;
    bool check = true;
    private bool connectFailed = false;

    void Awake()
    {
        PhotonNetwork.automaticallySyncScene = true;

        if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated)
        {
            PhotonNetwork.ConnectUsingSettings("0.1");
        }

        if (string.IsNullOrEmpty(PhotonNetwork.playerName))
        {
            PhotonNetwork.playerName = "Guest" + Random.Range(1, 100);
        }
    }

    void Update()
    {
        if (!PhotonNetwork.connected)
        {
            GroupPanelNoConnection.SetActive(true);
            if (PhotonNetwork.connecting)
            {
                ConnectionProblemInfo.text = "Connecting to: " + PhotonNetwork.ServerAddress + "\n";
            }
            else
            {
                ConnectionProblemInfo.text = "Not connected. Check console output. Detailed connection state: " + PhotonNetwork.connectionStateDetailed + " Server: " + PhotonNetwork.ServerAddress;
            }

            if (this.connectFailed)
            {
                ConnectionProblemInfo.text = "Connection failed. Check setup and use Setup Wizard to fix configuration. \n";
                ConnectionProblemInfo.text += string.Format("Server: {0}", new object[] { PhotonNetwork.ServerAddress }) + "\n";
                ConnectionProblemInfo.text += "AppId: " + PhotonNetwork.PhotonServerSettings.AppID.Substring(0, 8) + "****" + "\n";

                TryAgainButton.gameObject.SetActive(true);
            }

            return;
        }

        GroupPanelNoConnection.SetActive(false);
        if (check)
        {
            GroupPanelLobby.SetActive(true);
        }
    }

    public void TryAganinButton()
    {
        connectFailed = false;
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    public void CreateRoomHandler()
    {
        bool create = true;
        foreach (var item in PhotonNetwork.GetRoomList())
        {
            if (item.name == RoomNameField.text)
                create = false;
        }
        if (!create)
            return;

        PhotonNetwork.CreateRoom(RoomNameField.text, new RoomOptions { maxPlayers = 5 }, null);

        PhotonNetwork.playerName = NicknameField.text;
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
    }

    public void JoinRoomHandler()
    {
        PhotonNetwork.JoinRoom(RoomNameField.text);
    }

    public void JoinRandomRoomHandler()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnFailedToConnectToPhoton(object parameters)
    {
        this.connectFailed = true;
    }

    public void OnCreatedRoom()
    {
        PhotonNetwork.LoadLevel("Main");
    }

    public void ShowPlayers()
    {
        check = false;
        GroupPanelLobby.SetActive(false);
        GoblinStalker.SetActive(true);
        ShowMenu.gameObject.SetActive(true);
        NextPlayerButton.gameObject.SetActive(true);
    }

    public void ActiveMenu()
    {
        check = true;
        ShowMenu.gameObject.SetActive(false);
        GoblinStalker.SetActive(false);
        GoblinSniper.SetActive(false);
        NextPlayerButton.gameObject.SetActive(false);
        InventoryPanel.SetActive(false);
    }

    public void NextPlayer()
    {
        if (GoblinStalker.activeSelf)
        {
            GoblinSniper.SetActive(true);
            GoblinStalker.SetActive(false);
        }
        else if (GoblinSniper.activeSelf)
        {
            GoblinSniper.SetActive(false);
            GoblinStalker.SetActive(true);
        }
    }

    public void ShowInventory()
    {
        SceneManager.LoadScene("Inventory");
    }
}
