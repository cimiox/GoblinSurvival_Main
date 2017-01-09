using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance = null;
    public List<Item> database = new List<Item>();
    private JsonData itemData;
    public TextAsset _textAsset;

    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            DestroyObject(this);

        itemData = JsonMapper.ToObject(_textAsset.text);

        ConstructItemDatabase();
    }

    public Item FetchItemById(int id)
    {
        for (int i = 0; i < database.Count; i++)
        {
            if (database[i].Id == id)
            {
                return database[i];
            }
        }
        return null;
    }

    private void ConstructItemDatabase()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            database.Add(new Item((int)itemData[i]["Id"], (string)itemData[i]["Title"], (int)itemData[i]["Value"], (string)itemData[i]["Slug"], (bool)itemData[i]["Stackable"]));
        }
    }
}

[Serializable]
public class Item
{
    public int Id { get; set; }
    public string Tittle { get; set; }
    public int Value { get; set; }
    public string Slug { get; set; }
    public bool Stackable { get; set; }
    public Sprite Icon { get; set; }

    public Item(int id, string tittle, int value, string slug, bool stackable)
    {
        this.Id = id;
        this.Tittle = tittle;
        this.Value = value;
        this.Stackable = stackable;
        Icon = Resources.Load<Sprite>("Sprites/" + slug);
    }

    public Item()
    {
        Id = -1;
    }
}
