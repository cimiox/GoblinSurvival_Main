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
    List<int> CountsItems = new List<int>();

    List<int> ItemCellId = new List<int>();

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
                LoadItemsInInventory();
            }
        }

        if (!InventoryPanel.activeSelf)
        {
            activeInventoryPanel = 0;
        }
    }

    public void LoadItemsInInventory()
    {
        CreateCells();

        ChangeState();

        ReadInFile();

        for (int i = 0; i < idItems.Count; i++)
        {
            AddItemInInventory(idItems[i]);
        }

        for (int i = 0; i < CountsItems.Count && i < itemsGameObjects.Count; i++)
        {
            if (CountsItems[i] == 0)
            {
                itemsGameObjects[i].ItemObj.GetComponent<ItemData>().amount = CountsItems[i];
            }
            else
            {
                itemsGameObjects[i].ItemObj.GetComponent<ItemData>().amount = CountsItems[i];
                itemsGameObjects[i].ItemObj.GetComponentInChildren<Text>().text = CountsItems[i].ToString();
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

                if (item.ChildNodes[1].InnerText == null || item.ChildNodes[1].InnerText == "")
                    CountsItems.Add(0);
                else
                    CountsItems.Add(int.Parse(item.ChildNodes[1].InnerText));
                
                ItemCellId.Add(Convert.ToInt32(item.ChildNodes[2].InnerText));
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

        foreach (var item in InventoryWindow.GetComponentsInChildren<ItemData>())
        {
            if (item._item.Id != -1)
            {
                itemNode = xmlDoc.CreateElement("Item");
                rootNode.AppendChild(itemNode);

                itemsNode = xmlDoc.CreateElement("Id");
                itemsNode.InnerText = item._item.Id.ToString();
                itemNode.AppendChild(itemsNode);

                itemsNode = xmlDoc.CreateElement("InStack");
                itemsNode.InnerText = item.gameObject.GetComponentInChildren<Text>().text;
                itemNode.AppendChild(itemsNode);

                itemsNode = xmlDoc.CreateElement("Slot");
                itemsNode.InnerText = item.gameObject.transform.parent.GetComponent<Cell>().id.ToString();
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
                slots.Add(Instantiate(inventoryCell));
                slots[i].GetComponent<Cell>().id = i;
                slots[i].transform.SetParent(cellPanel.transform);
                items.Add(new Item());
            }
        }
        else
        {
            for (int i = 0; i < countSlots; i++)
            {
                slots[i] = Instantiate(inventoryCell);
                slots[i].GetComponent<Cell>().id = i;
                slots[i].transform.SetParent(cellPanel.transform);
                items.Add(new Item());
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

        print("ID item to add - " + itemToAdd.Id);

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

    private void AddItemInInventory(int id, int slotID)
    {
        Item itemToAdd = database.FetchItemById(id);
        if (!CheckItemInCell(slots[slotID]))
        {
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
                        itemObj.GetComponent<ItemData>().cell = slotID;
                        itemObj.transform.SetParent(slots[slotID].transform);

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
    }

    private bool CheckItemInCell(GameObject slot)
    {
        if (slot.GetComponentInChildren<ItemData>())
        {
            return true;
        }

        return false;
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

public enum SceneState
{
    InLobby = 0,
    InGame = 1
}
