using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryDatabaseController : EditorWindow
{
    public GameObject asset;

    private string title;
    public string Title
    {
        get { return title; }
        set { title = value; }
    }

    private int valueItem;
    public int ValueItem
    {
        get { return valueItem; }
        set { valueItem = value; }
    }

    private Sprite slug;
    public Sprite Slug
    {
        get { return slug; }
        set { slug = value; }
    }

    private string[] Strings = new string[] { "1", "2"};
    private int selGridInt;

    [MenuItem("Window/My window")]
    private static void ShowWindow()
    {
        GetWindow(typeof(InventoryDatabaseController));
    }

    private void OnGUI()
    {
        EditorGUILayout.TextField("Title", title);
        EditorGUILayout.IntField("Value", valueItem);
        EditorGUILayout.ObjectField("Sprite", Slug, typeof(Sprite), true);
        GUILayout.Label("Stats");

        //ScriptableObject target = this;
        SerializedObject so = new SerializedObject(this);
        SerializedProperty stringsProperty = so.FindProperty("Strings");
        //EditorGUILayout.PropertyField(stringsProperty, "List", true);
        so.ApplyModifiedProperties();
    }
}
