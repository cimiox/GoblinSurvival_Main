using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Xml;
using System.IO;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance = null;
    GameObject inventoryPanel;
    GameObject cellPanel;

    GameObject cellPanelInGame;

    public GameObject inventoryCell;
    public GameObject inventoryItem;
    ItemDatabase database;

    public TextAsset preservationItems;

    private SceneState sceneState;
    public SceneState SceneState
    {
        get { return sceneState; }
        set
        {
            sceneState = value;
        }
    }

    private int countSlots = 6;
    public List<Item> items = new List<Item>();
    public List<GameObject> itemsGameObjects = new List<GameObject>();
    public List<GameObject> slots = new List<GameObject>();

    public List<Item> itemsInGame = new List<Item>();

    List<int> idItems = new List<int>();

    readonly string pathToFile = "Assets/Test.xml";

    void Start()
    {
        CreateCells();

        Singletone();

        ChangeState();

        ReadInFile();

        for (int i = 0; i < idItems.Count; i++)
        {
            AddItem(idItems[i]);
        }
    }

    public void ReadInFile()
    {
        if (File.Exists(pathToFile))
        {
            XmlTextReader reader = new XmlTextReader(pathToFile);

            while (reader.Read())
            {
                print("Вызывает бесконечно");
                if (reader.IsStartElement("Item"))
                {
                    idItems.Add(Convert.ToInt32(reader.ReadString()));
                }
            }

            reader.Close();
        }
    }

    public void WriteInFile()
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlNode rootNode = xmlDoc.CreateElement("Items");
        xmlDoc.AppendChild(rootNode);

        XmlNode itemNode;
        XmlAttribute itemAttribute;

        for (int i = 0; i < items.Count && i < itemsGameObjects.Count; i++)
        {
            if (items[i].Id != -1)
            {
                itemNode = xmlDoc.CreateElement("Item");
                itemAttribute = xmlDoc.CreateAttribute("ValueInStack");
                itemAttribute.Value = itemsGameObjects[i].GetComponentInChildren<Text>().text;
                itemNode.Attributes.Append(itemAttribute);
                itemNode.InnerText = items[i].Id.ToString();
                rootNode.AppendChild(itemNode);
            }
        }

        xmlDoc.Save(pathToFile);
    }

    private void CreateCells()
    {
        database = GetComponent<ItemDatabase>();
        inventoryPanel = GameObject.Find("InventoryPanel");
        cellPanel = inventoryPanel.transform.FindChild("InventoryWindow").gameObject;

        if (slots.Count != countSlots)
        {
            for (int i = 0; i < countSlots; i++)
            {
                items.Add(new Item());
                slots.Add(Instantiate(inventoryCell));
                slots[i].GetComponent<Cell>().id = i;
                slots[i].transform.SetParent(cellPanel.transform);
            }
        }
        else
        {
            for (int i = 0; i < countSlots; i++)
            {
                items.Add(new Item());
                slots[i] = Instantiate(inventoryCell);
                slots[i].GetComponent<Cell>().id = i;
                slots[i].transform.SetParent(cellPanel.transform);
            }
        }
    }

    void LateUpdate()
    {
        ChangeState();
    }

    public void AddItem(int id)
    {
        switch (SceneState)
        {
            case SceneState.InLobby:
                break;
            case SceneState.InInventory:
                AddItemInInventory(id);
                break;
            case SceneState.InGame:
                break;
        }
    }

    private SceneState ChangeState()
    {
    Label:
        int loadedLevel = SceneManagerHelper.ActiveSceneBuildIndex;
        switch (loadedLevel)
        {
            case 0:
                SceneState = SceneState.InLobby;
                return SceneState;
            case 1:
                SceneState = SceneState.InInventory;
                return SceneState;
            case 2:
                SceneState = SceneState.InGame;
                return SceneState;
            default:
                goto Label;
        }
    }

    public void AddItemInInventory(int id)
    {
        Item itemToAdd = database.FetchItemById(id);

        if (itemToAdd.Stackable && CheckItemInInventory(itemToAdd))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Id == id)
                {
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();

                    data.amount++;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();

                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Id == -1)
                {
                    items[i] = itemToAdd;
                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.GetComponent<ItemData>()._item = itemToAdd;
                    itemObj.GetComponent<ItemData>().cell = i;
                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.transform.position = Vector2.zero;
                    itemObj.GetComponent<Image>().sprite = itemToAdd.Icon;
                    itemObj.name = itemToAdd.Tittle;

                    itemsGameObjects.Add(itemObj);
                    break;
                }
            }
        }

    }

    private bool CheckItemInInventory(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == item.Id)
            {
                return true;
            }
        }
        return false;
    }

    private void Singletone()
    {
        if (Instance == null)
            Instance = this;
        else
            DestroyObject(this);
    }
}

public enum SceneState
{
    InLobby = 0,
    InInventory = 1,
    InGame = 2
}
