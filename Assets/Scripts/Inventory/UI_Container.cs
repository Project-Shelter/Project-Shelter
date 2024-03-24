using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UI.Toggle;

namespace ItemContainer
{
    public class UI_Container : UI_Section
    {
        private static Action<int, ItemVO> sendItem; //int : receive container
        private void ReceiveItem(int receiver, ItemVO item)
        {
            if (receiver != controller.number)
                return;
            
            GetItem(item);
        }
        
        public ContainerController controller { get; private set; }

        public UI_Slot[] slots { get; private set; }
        public int maxCapacity { get; private set; }
        private int number;

        public void Start()
        {
            number = int.Parse(gameObject.name.Substring(gameObject.name.Length - 1));
            controller = new ContainerController(number);
            maxCapacity = ItemDummyData.MaxCapacity[number];
            slots = new UI_Slot[maxCapacity];
            Init();
        }

        public void OnEnable()
        {
            OpenInventory();
        }
        public void OnDisable()
        {
            CloseInventory();
        }

        public override void Init()
        {
            base.Init();
            
            //버튼 Bind - Obj name : item_{i}
            string[] slotStr = new string[maxCapacity];
            for (int i = 0; i < maxCapacity; i++)
            {
                slotStr[i] = $"panelItem_{i}";
            }
            
            Bind<UI_Slot>(slotStr);
            for (int i = 0; i < maxCapacity; i++)
            {
                slots[i] = Get<UI_Slot>(i);
            }

            InitView();
        }

        public int LoadId(int slot)
        {
            return slots[slot].Item.id;
        }

        //아이템 획득
        public void GetItem(ItemVO item)
        {
            int overlapCount = ItemDummyData.ItemDB.data[item.id].overlapCount;

            for (int i = 0; i < item.Count / overlapCount; i++)
            {
                int slot = controller.AddItem(item.id, overlapCount);
                UpdateSlot(slot);
            }
            
            if (item.Count % overlapCount != 0)
            {
                int slot = controller.AddItem(item.id, item.Count % overlapCount);
                UpdateSlot(slot);
            }
        }

        //아이템 전송
        public void GiveItem(int count, int slot, int receiver)
        {
            sendItem.Invoke(receiver, new ItemVO(controller.container.slots[slot].id, count));
            ThrowItem(count, slot);
        }

        public void GiveItem(int receiver)
        {
            for (int i = 0; i < maxCapacity; i++)
            {
                if (slots[i].IsOn)
                {
                    GiveItem(controller.container.slots[i].Count, i, receiver);
                }
            }
        }

        //아이템 버리기
        public void ThrowItem(int count, int slot){
            controller.RemoveItem(slot, count);
            UpdateSlot(slot);
        }

        public void ThrowPickedItem()
        {
            for (int i = 0; i < maxCapacity; i++)
            {
                if (slots[i].IsOn)
                {
                    ThrowItem(controller.container.slots[i].Count, i);
                }
            }
        }

        //최초출력
        private void InitView()
        {
            for (int i = 0; i < maxCapacity; i++)
            {
                UpdateSlot(i);
            }
        }
        protected void OpenInventory()
        {
            sendItem -= ReceiveItem;
            sendItem += ReceiveItem;
        }
        
        //인벤토리 닫기
        protected void CloseInventory()
        {
            sendItem -= ReceiveItem;
            FlushItem();
        }

        //선택된 아이템 Flush
        private void FlushItem(){ }
        
        private void UpdateSlot(int slot)
        {
            if(controller.container.slots.ContainsKey(slot))
                slots[slot].UpdateSlot(controller.container.slots[slot]);
            else slots[slot].UpdateSlot(ItemDummyData.NullItem);
        }
        
    }
}