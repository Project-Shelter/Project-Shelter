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
    private ContainerModel inventory = ContainerInjector.InjectContainer(0);
    public List<CraftItem> GetCraftList(ItemKind itemKind)
    {
        List<CraftItem> list = new List<CraftItem>();
        foreach (var item in ItemDummyData.ItemDB.data)
        {
            if (item.Value.itemKind == itemKind)
            {
                list.Add(new CraftItem(item.Value));
            }
        }
        return list;
    }

    public void MakeCraftItem(int itemID)
    {
        if (!CanCraftItem(itemID)) return;
        //inventory.RemoveItem(); 아이템 삭제
        inventory.AddItem(itemID, 1);
    }

    public bool CanCraftItem(int itemID)
    {
        if (inventory.HasItem(new ItemVO(itemID, 1))) return true;
        return false;
    }
}
