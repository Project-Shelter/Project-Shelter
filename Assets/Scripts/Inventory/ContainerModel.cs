using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

namespace ItemContainer
{
    //Container Model
    public class ContainerModel
    {
        public Dictionary<int, ItemVO> slots { get; private set; }
        public int maxCapacity { get; set; }

        public Action AddItemAction = null;
        public Action RemoveItemAction = null;
        public ContainerModel(int num)
        {
            slots = new Dictionary<int, ItemVO>();
            maxCapacity = ItemDummyData.MaxCapacity[num];
            SetContainer(num);
        }

        public ContainerModel(Dictionary<int, ItemVO> slots, int maxCapacity)
        {
            this.slots = slots;
            this.maxCapacity = maxCapacity;
        }

        public void SetContainer(int num)
        {
            slots = ItemDummyData.invenSlots[num];
        }
        
        private int emptySlot
        {
            get
            {
                for (int i = 0; i < maxCapacity; i++)
                    if (!slots.ContainsKey(i))
                        return i;
                
                return -1;
            }
        }

        public int currentWeight
        {
            get
            {
                int weight = 0;
                for (int i = 0; i < maxCapacity; i++)
                {
                    if (slots.ContainsKey(i))
                    {
                        weight += slots[i].Count * ItemDummyData.ItemDB.data[slots[i].id].weight;
                    }
                }

                return weight;
            }
        }

        public int AddItem(int id, int count)
        {
            int slot = emptySlot;
            foreach (var item in slots)
            {
                if (item.Value.id == id && item.Value.Count + count <= ItemDummyData.ItemDB.data[id].overlapCount)
                {
                    slot = item.Key;
                    break;
                }
            }
            if (slot == -1)
                return -1;
            AddItem(id, count, slot);

            return slot;
        }
        
        public void AddItem(int id, int count, int slot)
        {
            //슬롯 범위 체크
            if(slot > maxCapacity)
            {
                Debug.Log("@WARNING@ 슬롯 범위 밖 슬롯 접근");
                return;
            }

            if (slots.ContainsKey(slot))
            {
                if (slots[slot].id == id) slots[slot].Count += count;
                else slots[slot] = new ItemVO(id, count);
            }
            else
            {
                slots.Add(slot, new ItemVO(id, count));
            }
            
            AddItemAction?.Invoke();
        }
        
        public void RemoveItem(int slot, int count)
        {
            //슬롯 범위 체크
            if(slot > maxCapacity)
            {
                Debug.Log("@WARNING@ 슬롯 범위 밖 슬롯 접근");
                return;
            }

            if (slots[slot].Count < count)
            {
                Debug.Log("@WARNING@ 인벤토리 frontend 단 작업 에러");
                return;
            }

            slots[slot].Count -= count;

            if (slots[slot].Count == 0)
            {
                slots.Remove(slot);
            }
            
            RemoveItemAction?.Invoke();
        }

        public void RemoveItem(ItemVO itemVo)
        {
            int count = itemVo.Count;
            foreach (var item in slots)
            {
                if (item.Value.id == itemVo.id)
                {
                    item.Value.Count -= count;
                    if (item.Value.Count < 0)
                    {
                        count = -item.Value.Count;
                        item.Value.Count = 0;
                    }
                    else return;
                }
            }
            
            RemoveItemAction?.Invoke();
        }
        
        public bool HasItem(ItemVO itemVo)
        {
            int count = itemVo.Count;
            foreach (var item in slots)
            {
                if (item.Value.id == itemVo.id)
                {
                    count -= item.Value.Count;
                    if (count <= 0)
                        return true;
                }
            }

            return false;
        }
    }
    

}