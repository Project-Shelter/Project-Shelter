using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ItemContainer
{
    public class UI_InvenSystem : UI_Container, IDropHandler
    {
        protected static Action<int> pickItem;
        public static bool[] openContainer { get; private set; } = new bool[3];
        protected static int dropedContainer = 0;
        protected static int pickedContainer { get; private set; } = 0;
        protected static UI_InvenSystem[] containers = new UI_InvenSystem[3];

        private int sendNumber = -1;
        
        private bool ReceiveItem(int receiver, ItemVO item)
        {
            if (receiver != sendNumber)
                return false;
            
            return GetItem(item);
        }
        
        private void SetContainerNumber()
        {
            InitContainer();
            
            Debug.Log(containerID + gameObject.name);

            if (containerID < 2) sendNumber = containerID;
            else sendNumber = 2;

            containers[sendNumber] = this;
        }
        public bool GetItem(ItemVO item)
        {
            int overlapCount = ItemDummyData.ItemDB.data[item.id].overlapCount;

            for (int i = 0; i < item.Count / overlapCount; i++)
            {
                int slot = controller.AddItem(item.id, overlapCount);
                if (slot is -1) return false;
                UpdateSlot(slot);
            }
            
            if (item.Count % overlapCount != 0)
            {
                int slot = controller.AddItem(item.id, item.Count % overlapCount);
                if (slot is -1) return false;
                UpdateSlot(slot);
            }

            return true;
        }

        //아이템 전송
        public void GiveItem(int count, int slot, int receiver)
        {
            bool temp = containers[receiver].GetItem(new ItemVO(controller.container.slots[slot].id, count));
            if(temp) ThrowItem(count, slot);
        }

        public void DropItem(int slot)
        {
            if (dropedContainer is -1)
            {
                ThrowItem(controller.container.slots[slot].Count, slot);
                return;
            }
            Debug.Log(dropedContainer);
            GiveItem(controller.container.slots[slot].Count, slot, dropedContainer);
            dropedContainer = -1;
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
        
        private void PickItem(int sender)
        {
            if(sendNumber != pickedContainer) FlushItem();
        }

        //아이템 버리기
        private void ThrowItem(int count, int slot){
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

        public override void Start()
        {
            SetContainerNumber();
            maxCapacity = ItemDummyData.MaxCapacity[sendNumber];
            base.Start();
        }
        
        public override void OnEnable()
        {
            if (sendNumber is -1)
            {
                SetContainerNumber();
            }
            
            openContainer[sendNumber] = true;
            base.OnEnable();
            
            pickItem -= PickItem;
            pickItem += PickItem;
        }
        
        public override void OnDisable()
        {
            openContainer[sendNumber] = false;
            pickItem -= PickItem;
            base.OnDisable();
        }

        public override void Init()
        {
            base.Init();

            for (int i = 0; i < maxCapacity; i++)
            {
                int slot = i;
                
                //아이템 선택
                slots[i].slotBtn.onValueChanged.AddListener((isOn) => {
                    if (isOn)
                    {
                        if (pickedContainer == sendNumber)
                            return;
                        
                        pickedContainer = sendNumber;

                        pickItem(sendNumber);
                    } });
                
                //- 버튼
                slots[i].subBtn.onClick.AddListener(delegate
                {
                    int receiver = (sendNumber == 2) ? 0 : 2;
                    if (receiver is 0 && containers[0].controller.container.slots.Count == maxCapacity) receiver = 1;
                    GiveItem(1, slot, receiver);
                });
                
                slots[i].DropItem -= DropItem;
                slots[i].DropItem += DropItem;
            }
        }
        
        public void OnDrop(PointerEventData EventData)
        {
            dropedContainer = sendNumber;
        }
    }
}
