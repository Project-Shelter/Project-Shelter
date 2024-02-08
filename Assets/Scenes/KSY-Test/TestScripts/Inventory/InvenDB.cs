using System.Collections.Generic;
using UnityEngine;
using System;

//[FE]인벤토리 각 슬롯의 아이템
public struct InvenItem
{
    public string name;
    public int count;

    public InvenItem(string _name = null, int _count = 1)
    {
        name = _name;
        count = _count;
    }
}

public class InvenDB
{
    #region Static Data

    public const int slotCount = 18;
    public const int quickSlotCount = 6;
    public Dictionary<string, Item> ItemDB = new Dictionary<string, Item>();

    #endregion
    
    #region Dynamic Data
    public int defaultMaxWeight { get; set; } = 100;
    public int currentBagExtension { get; set;} = 100;
    public int currentWeight { get; set;} = 0;

    //[BE]item id - 총 개수
    public Dictionary<string, int> Items = new Dictionary<string, int>();

    //[FE]index : 위치
    public InvenItem[] InvenItems = new InvenItem[slotCount];
    public InvenItem[] QuickItems = new InvenItem[quickSlotCount];
    
    #endregion
}
