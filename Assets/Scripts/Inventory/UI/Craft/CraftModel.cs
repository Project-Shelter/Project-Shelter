using System.Collections.Generic;
using ItemContainer;
using UnityEngine;
using UnityEngine.UI;

//불변객체
public class CraftItem
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public string Comment { get; private set; }
    public Sprite Icon { get; private set; }

    public CraftItem(ItemData data)
    {
        ID = data.ID;
        Name = data.name;
        Comment = data.description;
        Icon = data.image;
    }
}

public class CraftModel
{
    public List<CraftItem> GetCraftList(ItemKind itemKind)
    {
        List<CraftItem> list = new List<CraftItem>();
        foreach (var item in ItemDummyData.ItemDB.data)
        {
            if (item.Value.kind == itemKind)
            {
                list.Add(new CraftItem(item.Value));
            }
        }
        return list;
    }

    public void MakeCraftItem(int itemID)
    {
        if (!CanCraftItem(itemID)) return;
        //InvenSlot을 ContainerModel로 리팩토링 해야 함... 그래야 구현 가능.
    }

    public bool CanCraftItem(int itemID)
    {
        if (ItemDummyData.HasItem(new ItemVO(itemID, 1))) return true;
        return false;
    }
}
