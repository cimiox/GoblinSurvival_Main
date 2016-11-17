using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConnectionStatus : MonoBehaviour {
    public InputField InputFieldNickName;
    public Button ButtonNickName;

    void OnGUI()
    {
        float width = 400;
        float height = 100;

        Rect centeredRect = new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height);

        GUILayout.BeginArea(centeredRect, GUI.skin.box);
        {
            GUILayout.Label("Connecting" + GetConnectingDots(), GUI.skin.customStyles[0]);
            GUILayout.Label("Status: " + PhotonNetwork.connectionStateDetailed);
        }
        GUILayout.EndArea();

        if (PhotonNetwork.inRoom)
        {
            enabled = false;
        }

        //if (PhotonNetwork.connected)
        //{
        //    InputFieldNickName.gameObject.SetActive(true);
        //    ButtonNickName.gameObject.SetActive(true);
        //}
    }
    
    /// <summary>
    /// Get vonnection status
    /// </summary>
    /// <returns> Connection status</returns>
    string GetConnectingDots()
    {
        string str = "";
        int numberOfDots = Mathf.FloorToInt(Time.timeSinceLevelLoad * 3f % 4);

        for (int i = 0; i < numberOfDots; ++i)
        {
            str += " .";
        }

        return str;
    }
}
