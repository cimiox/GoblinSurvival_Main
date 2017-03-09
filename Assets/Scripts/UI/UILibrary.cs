using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILibrary : MonoBehaviour
{

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


}
