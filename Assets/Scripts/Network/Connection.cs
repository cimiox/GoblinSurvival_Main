using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Connection : Photon.PunBehaviour
{
    const string ConnectSetting = "v0.1";

    private delegate void GettingLog();
    private event GettingLog GetLog;

    private static string logError;
    public static string LogError
    {
        get { return logError; }
        set { logError = value; }
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
    public static void GetLogError(GameObject textObj)
    {
        if (textObj.GetComponent<Text>() != null)
        {
            textObj.GetComponent<Text>().text = LogError;
        }
    }

    public static void LoadConnectionProblemPanel()
    {
        GameObject noConnectionPanel = Instantiate(Resources.Load("UI/NoConnection") as GameObject);
        noConnectionPanel.transform.SetParent(GameObject.Find("Canvas").transform);
        //noConnectionPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500);

        noConnectionPanel.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        noConnectionPanel.GetComponent<RectTransform>().offsetMin = Vector2.zero;

        noConnectionPanel.GetComponent<RectTransform>().localScale = Vector3.one;

        noConnectionPanel.transform.FindChild("ConnectionProblemInfo").GetComponent<Text>().text = LogError;
    }
}
