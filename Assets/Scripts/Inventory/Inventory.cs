using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using System.ComponentModel;

public class Inventory : MonoBehaviour
{
    #region Events


    #endregion Events

    public GameObject InventoryPanel;

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

    public List<ItemObject> itemsGameObjects = new List<ItemObject>();
    public List<GameObject> slots = new List<GameObject>();

    List<int> idItems = new List<int>();
    List<string> CountsItems = new List<string>();

    public GameObject InventoryWindow;

    public string pathToFile
    {
        get
        {
#if UNITY_EDITOR
            return "Assets/Resources/Test.xml";
#elif UNITY_ANDROID
            return Application.persistentDataPath + "Test.xml";
#endif
        }
    }

    int activeInventoryPanel = 0;

    void Start()
    {
        Singletone();
    }

    void LateUpdate()
    {
        ChangeState();

        if (InventoryPanel.activeSelf && activeInventoryPanel == 0)
        {
            activeInventoryPanel++;

            if (InventoryWindow.GetComponentsInChildren<Cell>().Length < countSlots)
            {
                AddItemsInInventory();
            }
        }

        if (!InventoryPanel.activeSelf)
        {
            activeInventoryPanel = 0;
        }
    }

    public void AddItemsInInventory()
    {
        CreateCells();

        ChangeState();

        ReadInFile();

        for (int i = 0; i < idItems.Count; i++)
        {
            AddItem(idItems[i]);
        }

        for (int i = 0; i < CountsItems.Count && i < itemsGameObjects.Count; i++)
        {
            if (CountsItems[i] != null)
            {
                itemsGameObjects[i].ItemObj.GetComponent<ItemData>().amount = Convert.ToInt32(CountsItems[i].Trim());
                itemsGameObjects[i].ItemObj.GetComponentInChildren<Text>().text = CountsItems[i];
            }
        }
    }

    public void ReadInFile()
    {
        if (File.Exists(pathToFile))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(pathToFile);

            foreach (XmlElement item in doc.DocumentElement.ChildNodes)
            {
                idItems.Add(int.Parse(item.ChildNodes[0].InnerText));
                CountsItems.Add(item.ChildNodes[1].InnerText);
            }
        }
    }

    public void WriteInFile()
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlNode rootNode = xmlDoc.CreateElement("Items");
        xmlDoc.AppendChild(rootNode);

        XmlNode itemNode;
        XmlNode itemsNode;

        foreach (var item in itemsGameObjects)
        {
            if (item.ID != -1)
            {
                itemNode = xmlDoc.CreateElement("Item");
                rootNode.AppendChild(itemNode);

                itemsNode = xmlDoc.CreateElement("Id");
                itemsNode.InnerText = item.ID.ToString();
                itemNode.AppendChild(itemsNode);

                itemsNode = xmlDoc.CreateElement("InStack");
                itemsNode.InnerText = item.ItemObj.GetComponentInChildren<Text>().text;
                itemNode.AppendChild(itemsNode);
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



    public void AddItem(int id)
    {
        switch (SceneState)
        {
            case SceneState.InLobby:
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
                SceneState = SceneState.InGame;
                return SceneState;
            default:
                goto Label;
        }
    }

    private void AddItemInInventory(int id)
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

                    itemObj.GetComponent<RectTransform>().offsetMin = Vector2.zero;
                    itemObj.GetComponent<RectTransform>().offsetMax = Vector2.zero;

                    itemObj.GetComponent<Image>().sprite = itemToAdd.Icon;
                    itemObj.name = itemToAdd.Tittle;

                    itemsGameObjects.Add(new ItemObject(itemToAdd.Id, itemObj));
                    break;
                }
            }
        }

        WriteInFile();
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

        DontDestroyOnLoad(this);
    }

    public class ItemObject
    {
        public int ID { get; set; }
        public GameObject ItemObj { get; set; }

        public ItemObject(int id, GameObject item)
        {
            this.ID = id;
            this.ItemObj = item;
        }
    }

    public void AddRandomItem()
    {
        System.Random rand = new System.Random();
        int id = rand.Next(0, 2);
        AddItemInInventory(id);
    }
}

public class ItemGameObject
{
    Item item;
    GameObject GOItem;

    public ItemGameObject(Item _item, GameObject _gOItem)
    {
        this.item = _item;
        this.GOItem = _gOItem;
    }
}

public enum SceneState
{
    InLobby = 0,
    InGame = 1
}
