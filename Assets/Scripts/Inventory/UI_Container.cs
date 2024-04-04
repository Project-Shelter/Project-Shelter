using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UI.Toggle;

namespace ItemContainer
{
    public class UI_Container : UI_Section
    {
        protected static bool[] openContainer { get; private set; } = new bool[3];
        protected static int pickedContainer { get; private set; } = 0;

        private static Action<int, ItemVO> sendItem; //int : receive container
        protected static Action<int> pickItem;
        private void ReceiveItem(int receiver, ItemVO item)
        {
            if (receiver != sendNumber)
                return;
            
            GetItem(item);
        }
        private void PickItem(int sender)
        {
            if(sendNumber != pickedContainer) FlushItem();
        }
        
        public ContainerController controller { get; private set; }

        public UI_Slot[] slots { get; private set; }
        public int maxCapacity { get; private set; }
        protected int number = -1;
        private int sendNumber = -1;

        public virtual void Start()
        {
            if(number == -1) number = int.Parse(gameObject.name.Substring(gameObject.name.Length - 1));
            controller = new ContainerController(number);

            if (number < 2) sendNumber = number;
            else sendNumber = 2;
            
            maxCapacity = ItemDummyData.MaxCapacity[sendNumber];
            
            slots = new UI_Slot[maxCapacity];
            
            Init();
            
            Debug.Log("Start" + sendNumber);
        }

        public virtual void OnEnable()
        {
            if (sendNumber is -1)
            {
                if(number == -1) number = int.Parse(gameObject.name.Substring(gameObject.name.Length - 1));
                controller = new ContainerController(number);

                if (number < 2) sendNumber = number;
                else sendNumber = 2;
            }
            openContainer[sendNumber] = true;
            OpenInventory();
            
            Debug.Log("OnEnable" + sendNumber);

        }
        public virtual void OnDisable()
        {
            openContainer[sendNumber] = false;
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
                slots[i].slotBtn.onValueChanged.AddListener((isOn) => {
                    if (isOn)
                    {
                        if (pickedContainer == sendNumber)
                            return;
                        
                        pickedContainer = sendNumber;

                        pickItem(sendNumber);
                    } });
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
        private void OpenInventory()
        {
            controller?.SetContainer(number);
            InitView();
            sendItem -= ReceiveItem;
            sendItem += ReceiveItem;
            pickItem -= PickItem;
            pickItem += PickItem;
        }
        
        //인벤토리 닫기
        private void CloseInventory()
        {
            sendItem -= ReceiveItem;
            pickItem -= PickItem;
            FlushItem();
        }

        //선택된 아이템 Flush
        private void FlushItem()
        {
            for (int i = 0; i < maxCapacity; i++) { slots[i].TurnOff(); }
        }
        
        private void UpdateSlot(int slot)
        {
            if(controller.container.slots.ContainsKey(slot))
                slots[slot].UpdateSlot(controller.container.slots[slot]);
            else slots[slot].UpdateSlot(ItemDummyData.NullItem);
        }
        
    }
}