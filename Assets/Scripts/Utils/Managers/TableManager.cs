using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class TableManager
{
    public Dictionary<int, Item> itemDatas { get; private set; } = new Dictionary<int, Item>();

    public void Init()
    {
        itemDatas = LoadJson<ItemTable, int, Item>("ItemTable").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resources.Load<TextAsset>($"Tables/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}