using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance = null;
    GameObject inventoryPanel;
    GameObject cellPanel;
    public GameObject inventoryCell;
    public GameObject inventoryItem;
    ItemDatabase database;

    public static EventHandler OnChangeScene = delegate { };

    private SceneState sceneState;
    public SceneState SceneState
    {
        get { return sceneState; }
        set
        {
            sceneState = value;
            OnChangeScene(null, EventArgs.Empty);
        }
    }

    private int countSlots = 6;
    public List<Item> items = new List<Item>();
    public List<GameObject> itemsGameObject = new List<GameObject>();
    public List<GameObject> slots = new List<GameObject>();

    void Start()
    {
        Singletone();

        OnChangeScene += CreateCells;
    }

    void CreateCells(object sender, EventArgs e)
    {
        print("Вызвало не один раз");

        database = GetComponent<ItemDatabase>();
        inventoryPanel = GameObject.Find("InventoryPanel");
        cellPanel = inventoryPanel.transform.FindChild("InventoryWindow").gameObject;

        for (int i = 0; i < countSlots; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventoryCell));
            slots[i].GetComponent<Cell>().id = i;
            slots[i].transform.SetParent(cellPanel.transform);
        }

        Tooltip.Instance.tooltip = GameObject.Find("Tooltip");
        Tooltip.Instance.tooltip.SetActive(false);

        AddItem(0);
        AddItem(0);
        AddItem(1);

        OnChangeScene -= CreateCells;
    }

    void LateUpdate()
    {
        ChangeState();
        print(sceneState);
    }

    public void AddItem(int id)
    {
        switch (SceneState)
        {
            case SceneState.InLobby:
                break;
            case SceneState.InInventory:
                AddItemInInventory(id);
                OnChangeScene += CreateCells;
                break;
            case SceneState.InGame:
                AddItemInGame(id);
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

    public void AddItemInGame(int id)
    {
        for (int i = 0; i < itemsGameObject.Count; i++)
        {
            GameObject itemObj = Instantiate(inventoryItem);
            itemObj.GetComponent<ItemData>()._item = itemsGameObject[i].GetComponent<Item>();
            itemObj.GetComponent<ItemData>().cell = i;
            itemObj.transform.SetParent(slots[i].transform);
            itemObj.transform.position = Vector2.zero;
            itemObj.GetComponent<Image>().sprite = itemsGameObject[i].GetComponent<Item>().Icon;
            itemObj.name = itemsGameObject[i].GetComponent<Item>().Tittle;
        }

        OnChangeScene -= CreateCells;
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

                    itemsGameObject.Add(itemObj);

                    

                    break;
                }
            }
        }

        OnChangeScene -= CreateCells;
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
}

public enum SceneState
{
    InLobby = 0,
    InInventory = 1,
    InGame = 2
}
