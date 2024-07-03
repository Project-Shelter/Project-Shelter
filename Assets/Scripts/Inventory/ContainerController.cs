using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

namespace ItemContainer
{
    //기본적인 인벤토리 기능(Repository)
    public class ContainerController
    {
        public ContainerVO container { get; protected set; }
        public int number { get; protected set; }
        public ContainerController(int num)
        {
            container = new ContainerVO(ItemDummyData.MaxCapacity[num]);
            SetContainer(num);
        }

        public void SetContainer(int num)
        {
            number = num;
            container.slots = ItemDummyData.invenSlots[num];
        }
        
        private int emptySlot
        {
            get
            {
                for (int i = 0; i < container.maxCapacity; i++)
                    if (!container.slots.ContainsKey(i))
                        return i;
                
                return -1;
            }
        }

        public int currentWeight
        {
            get
            {
                int weight = 0;
                for (int i = 0; i < container.maxCapacity; i++)
                {
                    if (container.slots.ContainsKey(i))
                    {
                        weight += container.slots[i].Count * ItemDummyData.ItemDB.data[container.slots[i].id].weight;
                    }
                }

                return weight;
            }
        }

        public int AddItem(int id, int count)
        {
            int slot = emptySlot;
            foreach (var item in container.slots)
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
            if(slot > container.maxCapacity)
            {
                Debug.Log("@WARNING@ 슬롯 범위 밖 슬롯 접근");
                return;
            }

            if (container.slots.ContainsKey(slot))
            {
                if (container.slots[slot].id == id) container.slots[slot].Count += count;
                else container.slots[slot] = new ItemVO(id, count);
            }
            else
            {
                container.slots.Add(slot, new ItemVO(id, count));
            }
        }
        
        public void RemoveItem(int slot, int count)
        {
            //슬롯 범위 체크
            if(slot > container.maxCapacity)
            {
                Debug.Log("@WARNING@ 슬롯 범위 밖 슬롯 접근");
                return;
            }

            if (container.slots[slot].Count < count)
            {
                Debug.Log("@WARNING@ 인벤토리 frontend 단 작업 에러");
                return;
            }

            container.slots[slot].Count -= count;

            if (container.slots[slot].Count == 0)
            {
                container.slots.Remove(slot);
            }
        }
    }
}