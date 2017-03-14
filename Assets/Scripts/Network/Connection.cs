using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Connection : Photon.PunBehaviour
{
    private const string ConnectSetting = "v0.1";

    private static string logError;
    public static string LogError
    {
        get { return logError; }
        set { logError = value; }
    }

    public static string PlayerName
    {
        get
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerName")))
                return PhotonNetwork.playerName = PlayerPrefs.GetString("PlayerName");
            else
                return PhotonNetwork.playerName;
        }
        private set
        {
            PhotonNetwork.playerName = value;
            PlayerPrefs.SetString("PlayerName", PhotonNetwork.playerName);
        }
    }


    /// <summary>
    /// Create connect to Photon Cloud
    /// </summary>
    public static void Connect()
    {
        if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated)
        {
            PhotonNetwork.ConnectUsingSettings(ConnectSetting);
        }
    }

    /// <summary>
    /// Overide method PunBehavior
    /// </summary>
    /// <param name="error"></param>
    private void OnFailedToConnect(NetworkConnectionError error)
    {
        LogError += error + "\n";
    }

    /// <summary>
    /// Give error log
    /// </summary>
    /// <param name="textObj"></param>
    public static string GetLogError()
    {
        if (LogError.Trim().Length != 0)
            return LogError;
        else
            return "Никаких ошибок";
    }

    /// <summary>
    /// Method for Login panel, send the player name
    /// </summary>
    /// <param name="input">Field for playerName</param>
    /// <param name="text">UI for player name</param>
    public void CreatePlayerName(InputField input)
    {
        PlayerName = input.text;
    }

    /// <summary>
    /// Intialize all UI
    /// </summary>
    protected void Intialize()
    {
        Connect();
        UILibrary.GetCanvasChildrens();

        if (string.IsNullOrEmpty(PlayerName))
            UILibrary.ShowElement(UILibrary.LoggingUI);

        UILibrary.ShowElement(UILibrary.LobbyUI);
        UILibrary.LobbyUI.transform.FindChild("Nickname").GetComponent<Text>().text = "Nickname: " + PlayerName;
    }
}
