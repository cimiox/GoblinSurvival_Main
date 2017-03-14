using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : Connection
{
    public InputField RoomNameField;
    public Button CreateRoom;
    public Button JoinRoom;
    public Button JoinRandomRoom;
    public Button TryAgainButton;
    public Button ShowPlayersButton;
    public Button ShowMenu;
    public Button NextPlayerButton;
    public Text ConnectionProblemInfo;
    public GameObject GoblinStalker;
    public GameObject GoblinSniper;

    private void Awake()
    {
        Intialize();
    }

    public void CreateRoomHandler()
    {
        bool create = true;
        foreach (var item in PhotonNetwork.GetRoomList())
        {
            if (item.Name == RoomNameField.text)
                create = false;
        }
        if (!create)
            return;

        PhotonNetwork.CreateRoom(RoomNameField.text, new RoomOptions { MaxPlayers = 5 }, null);
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

    public void OnCreatedRoom()
    {
        PhotonNetwork.LoadLevel("Main");
    }

    public void ShowPlayers()
    {
        GoblinStalker.SetActive(true);
        ShowMenu.gameObject.SetActive(true);
        NextPlayerButton.gameObject.SetActive(true);
    }

    public void ActiveMenu()
    {
        ShowMenu.gameObject.SetActive(false);
        GoblinStalker.SetActive(false);
        GoblinSniper.SetActive(false);
        NextPlayerButton.gameObject.SetActive(false);
    }
}
