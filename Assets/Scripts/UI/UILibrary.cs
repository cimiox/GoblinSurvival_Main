using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILibrary : MonoBehaviour
{
    private static GameObject lobbyUI;
    public static GameObject LobbyUI
    {
        get
        {
            if (lobbyUI == null)
                return lobbyUI = GetCanvasChild("GroupLobbyUI");
            else
                return lobbyUI;
        }
    }

    private static GameObject loggingUI;
    public static GameObject LoggingUI
    {
        get
        {
            if (loggingUI == null)
                return loggingUI = GetCanvasChild("LoggingPanel");
            else
                return loggingUI;
        }
    }

    private static GameObject noConnectionPanel;
    public static GameObject NoConnectionPanel
    {
        get
        {
            if (noConnectionPanel == null)
                return noConnectionPanel = GetCanvasChild("ConnectionProblemInfo");
            else
                return noConnectionPanel;
        }
    }

    private static Dictionary<string, GameObject> canvasChildren = new Dictionary<string, GameObject>();
    public static Dictionary<string, GameObject> CanvasChildren
    {
        get { return canvasChildren; }
        set { canvasChildren = value; }
    }

    private static GameObject canvasGO;
    private static GameObject CanvasGO
    {
        get
        {
            if (canvasGO == null)
                return canvasGO = GameObject.Find("Canvas");
            else
                return canvasGO;
        }
    }

    /// <summary>
    /// Set UI element True or False
    /// </summary>
    /// <param name="element"></param>
    public static void ShowElement(GameObject element)
    {
        switch (element.activeSelf)
        {
            case true:
                element.SetActive(false);
                break;
            case false:
                element.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Create panel connection problem
    /// </summary>
    /// <param name="log"></param>
    public static void LoadConnectionProblemPanel(string log)
    {
        ShowElement(NoConnectionPanel);
        noConnectionPanel.transform.FindChild("ConnectionProblemInfo").GetComponent<Text>().text = log;
    }

    /// <summary>
    /// Set parent canvas and set offsetMax Vector2.zero
    /// </summary>
    /// <param name="ui">ui element</param>
    /// <returns></returns>
    public static GameObject InstantiateUI(GameObject ui)
    {
        ui = Instantiate(ui);

        ui.transform.SetParent(GameObject.Find("Canvas").transform);

        ui.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        ui.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        ui.GetComponent<RectTransform>().localScale = Vector3.one;

        return ui;
    }

    /// <summary>
    /// Set parent canvas and offsetMax Vector2.zero
    /// </summary>
    /// <param name="path">path to resources load</param>
    /// <returns></returns>
    public static GameObject InstantiateUI(string path)
    {
        GameObject ui = Instantiate(Resources.Load(path) as GameObject);

        ui.transform.SetParent(GameObject.Find("Canvas").transform);

        ui.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        ui.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        ui.GetComponent<RectTransform>().localScale = Vector3.one;

        return ui;
    }

    /// <summary>
    /// Get canvas childrens
    /// </summary>
    public static void GetCanvasChildrens()
    {
        if (CanvasChildren.Count > 0)
        {
            foreach (var item in CanvasChildren)
            {
                CanvasChildren.Remove(item.Key);
            }
        }

        for (int i = 0; i < CanvasGO.transform.childCount - 1; i++)
        {
            CanvasChildren.Add(CanvasGO.transform.GetChild(i).name, CanvasGO.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Give object in Canvas
    /// </summary>
    /// <param name="name">name object</param>
    /// <returns></returns>
    public static GameObject GetCanvasChild(string name)
    {
        if (CanvasChildren.Count == 0)
            GetCanvasChildrens();

        foreach (var item in CanvasChildren)
        {
            if (item.Key == name)
            {
                return item.Value;
            }
        }

        return null;
    }
}

