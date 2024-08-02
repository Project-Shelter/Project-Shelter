using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Item
[Serializable]
public class Item
{
    public enum ItemType
    {
        NULL = 0,
        UseItem,
        EtcItem,
        ActionItem,
        EquipItem,
    }
    public int Item_ID;
    public string Item_Name;
    public int Item_Description;
    public ItemType Item_Type;
    public int Item_Weight;
    public int Item_Skill_ID;
    public int Item_Min_Dmg;
    public int Item_Max_Dmg;
    public int Item_OverlapCount;
}

[Serializable]
public class ItemTable : ILoader<int, Item>
{
    public List<Item> Items = new List<Item>();

    public Dictionary<int, Item> MakeDict()
    {
        Dictionary<int, Item> dict = new Dictionary<int, Item>();
        foreach (Item item in Items) { dict.Add(item.Item_ID, item); }
        return dict;
    }
}

#endregion