using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chest
{
    
    public Action<int> UpdateSlot = null;
    
    private const int slotCount = 12;
    
    public Dictionary<string, int> Items = new Dictionary<string, int>();
    public InvenItem[] ChestItems = new InvenItem[slotCount];
    
    public Item getItemBySlot(int slot)
    {
        if (ChestItems[slot].name == null) return null;
        return Inventory.Instance.DB.ItemDB[ChestItems[slot].name];
    }
    public InvenItem getInvenItemBySlot(int slot) { return ChestItems[slot];}
    
    public bool hasItem(string id) { return Items.ContainsKey(id); }
    public int emptyInvenSlot
    {
        get
        {
            for (int i = 0; i < ChestItems.Length; i++)
                if (ChestItems[i].name == null)
                    return i;
            return -1;
        }
    }

    
    public void AddItem(string item, int count = 1)
    {
        if (!hasItem(item))
        {
            Items.Add(item, 0);
        }

        Items[item] += count;

        for (int i = 0; i < ChestItems.Length; i++)
        {
            InvenItem _item = getInvenItemBySlot(i); //ref?
            if (_item.name == null)
                continue;
            
            int overlapCount = Inventory.Instance.DB.ItemDB[item].overlapCount;
            
            if (_item.name == item && _item.count < overlapCount)
            {
                int margin = overlapCount - _item.count;
                
                _item.count += (margin > count) ? count : margin;

                count -= margin;

                ChestItems[i].count = _item.count;

                UpdateSlot?.Invoke(i);

            }
            if (count <= 0)
                break;
        }

        while (count > 0)
        {
            int slot = emptyInvenSlot;
            if (slot == -1)
                return;
            
            int overlapCount = Inventory.Instance.DB.ItemDB[item].overlapCount;
            
            int value = (overlapCount > count) ? count : overlapCount;

            ChestItems[slot] = new InvenItem(item, value); //overlap 처리..

            count -= value;
            
            UpdateSlot?.Invoke(slot);
        }
    }

    public void RemoveItem(int slot)
    {
        InvenItem item = ChestItems[slot];
        
        if (!hasItem(item.name))
        {
            Debug.Log("@ERROR@ Somehow player is removing item in odd way!");
            return;
        }

        Items[item.name] -= item.count;
        
        ClearSlot(slot);
    }
    
        //슬롯 비우기
        private void ClearSlot(int slot)
        {
            ChestItems[slot] = new InvenItem();
        }
}
