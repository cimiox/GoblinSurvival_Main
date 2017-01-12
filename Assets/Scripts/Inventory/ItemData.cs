using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Item _item;
    public int amount;
    public int cell;

    private Inventory _inventory;
    private Vector2 offset;

    private Tooltip _tooltip;

    void Start()
    {
        if (SceneManagerHelper.ActiveSceneName == "Lobby")
        {
            _inventory = GameObject.Find("InventoryM").GetComponent<Inventory>();
        }
        else
        {
            _inventory = GameObject.Find("InventoryG").GetComponent<Inventory>();
        }
        

        if (_inventory.SceneState == SceneState.InLobby)
        {
            _tooltip = _inventory.GetComponent<Tooltip>();
        }

        if (amount != 0)
        {
            gameObject.GetComponentInChildren<Text>().text = amount.ToString();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_item != null)
        {
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
            this.transform.SetParent(this.transform.parent.parent);
            this.transform.position = eventData.position - offset;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_item != null)
        {
            this.transform.position = eventData.position - offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(_inventory.slots[cell].transform);
        this.transform.position = _inventory.slots[cell].transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        _inventory.WriteInFile();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_inventory.SceneState == SceneState.InLobby)
        {
            _tooltip.Active(_item);
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_inventory.SceneState == SceneState.InLobby)
        {
            _tooltip.Deactive();
        }
    }
}
