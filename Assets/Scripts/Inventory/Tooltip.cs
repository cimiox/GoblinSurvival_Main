using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public static Tooltip Instance = null;

    public Item item;
    private string data;
    public GameObject tooltip;

    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            DestroyObject(this);

    }

    void Update()
    {
        if (tooltip.activeSelf)
        {
            tooltip.transform.position = Input.mousePosition;
        }
    }

    public void Active(Item item)
    {
        this.item = item;
        ConstructDataString();
        tooltip.SetActive(true);
    }

    public void Deactive()
    {
        tooltip.SetActive(false);
    }

    public void ConstructDataString()
    {
        data = item.Id.ToString() + item.Tittle;
        tooltip.transform.GetChild(0).GetComponent<Text>().text = data;
    }
}
