using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IDropHandler
{
    public int id;
    private Inventory _inventory;

    public void OnDrop(PointerEventData eventData)
    {
        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();

        if (_inventory.items[id].Id == -1)
        {
            _inventory.items[droppedItem.cell] = new Item();
            _inventory.items[id] = droppedItem._item;

            droppedItem.cell = id;
        }
        else
        {
            Transform item = this.transform.GetChild(0);
            item.GetComponent<ItemData>().cell = droppedItem.cell;
            item.transform.SetParent(_inventory.slots[droppedItem.cell].transform);
            item.transform.position = _inventory.slots[droppedItem.cell].transform.position;

            droppedItem.cell = id;
            droppedItem.transform.SetParent(this.transform);
            droppedItem.transform.position = this.transform.position;

            _inventory.items[droppedItem.cell] = item.GetComponent<ItemData>()._item;
            _inventory.items[id] = droppedItem._item;
        }
    }

    void Start()
    {
        _inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

}
