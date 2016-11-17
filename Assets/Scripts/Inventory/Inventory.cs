using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    GameObject inventoryPanel;
    GameObject cellPanel;
    public GameObject inventoryCell;
    public GameObject inventoryItem;
    ItemDatabase database;

    public int countSlots = 10;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();


    void Start()
    {
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

        AddItem(0);
        AddItem(0);
        AddItem(1);
        AddItem(1);
    }

    public void AddItem(int id)
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

                    break;
                }
            }
        }
    }

    bool CheckItemInInventory(Item item)
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
}
