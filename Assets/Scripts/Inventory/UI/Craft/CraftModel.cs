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

    public CraftItem()
    {
        ID = 0;
        Name = "";
        Comment = "";
        Icon = null;
    }
}

public class CraftModel
{
    private ContainerModel inventory = ContainerInjector.InjectContainer(0);
    
    public List<CraftItem> GetCraftList(ItemKind itemKind)
    {
        List<CraftItem> list = new List<CraftItem>();
        foreach (var item in ItemDummyData.CraftItemDatas)
        {
            ItemData itemData = ItemDummyData.ItemDB.data[item.Value.ID];
            if (itemData.itemKind == itemKind)
            {
                list.Add(new CraftItem(itemData));
            }
        }
        return list;
    }

    public void MakeCraftItem(int itemID)
    {
        if (!CanCraftItem(itemID)) return;
        foreach (var material in ItemDummyData.CraftItemDatas[itemID].materials)
        {
            inventory.RemoveItem(material);
        }
        inventory.AddItem(itemID, 1);
    }

    public bool CanCraftItem(int itemID)
    {
        foreach (var material in ItemDummyData.CraftItemDatas[itemID].materials)
        {
            if (inventory.HasItem(material)) continue;
            return false;
        }
        return true;
    }

    public Sprite[] MaterialsIcon(int itemID)
    {
        int count = 8;
        Sprite[] Icons = new Sprite[count];

        int iconSlot = 0;
        for (int i = 0; i < ItemDummyData.CraftItemDatas[itemID].materials.Count; i++)
        {
            int materialCount = ItemDummyData.CraftItemDatas[itemID].materials[i].Count;
            Sprite sprite = ItemDummyData.ItemDB.data[ItemDummyData.CraftItemDatas[itemID].materials[i].id].image;
            for (int j = 0; j < materialCount; j++)
            {
                Icons[iconSlot] = sprite;
                iconSlot++;
            }
        }

        return Icons;
    }
}
