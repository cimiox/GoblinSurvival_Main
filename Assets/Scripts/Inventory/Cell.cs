using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IDropHandler
{
    public int id;

    public void OnDrop(PointerEventData eventData)
    {
        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();

        if (Inventory.Instance.items[id].Id == -1)
        {
            Inventory.Instance.items[droppedItem.cell] = new Item();
            Inventory.Instance.items[id] = droppedItem._item;
            droppedItem.cell = id;
        }
        else if (droppedItem.cell != id)
        {
            Transform item = this.transform.GetChild(0);
            item.GetComponent<ItemData>().cell = droppedItem.cell;
            item.transform.SetParent(Inventory.Instance.slots[droppedItem.cell].transform);
            item.transform.position = Inventory.Instance.slots[droppedItem.cell].transform.position;

            
            droppedItem.transform.SetParent(this.transform);
            droppedItem.transform.position = this.transform.position;

            Inventory.Instance.items[droppedItem.cell] = item.GetComponent<ItemData>()._item;
            Inventory.Instance.items[id] = droppedItem._item;
            droppedItem.cell = id;
        }
    }
}
