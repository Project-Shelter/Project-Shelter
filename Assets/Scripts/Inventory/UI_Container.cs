using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UI.Toggle;

namespace ItemContainer
{
    public class UI_Container : UI_Section, IDropHandler
    {
        private static UI_Container[] containers = new UI_Container[3];
        public static bool[] openContainer { get; private set; } = new bool[3];
        protected static int pickedContainer { get; private set; } = 0;

        //private static Action<int, ItemVO> sendItem; //int : receive container
        protected static Action<int> pickItem;

        private static Func<int, ItemVO, bool> sendItem;

        protected static int dropedContainer = 0;

        private bool ReceiveItem(int receiver, ItemVO item)
        {
            if (receiver != sendNumber)
                return false;
            
            return GetItem(item);
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
            SetContainerNumber();
            
            maxCapacity = ItemDummyData.MaxCapacity[sendNumber];
            slots = new UI_Slot[maxCapacity];
            
            Init();
        }

        public virtual void OnEnable()
        {
            if (sendNumber is -1)
            {
                SetContainerNumber();
            }
            
            openContainer[sendNumber] = true;
            OpenInventory();
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
                if (i / 10 is 1)
                    slotStr[i] = $"panelItem_{i}";
                else slotStr[i] = $"panelItem_0{i}";
            }
            
            Bind<UI_Slot>(slotStr);
            for (int i = 0; i < maxCapacity; i++)
            {
                int slot = i;
                slots[i] = Get<UI_Slot>(i);
                
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

            InitView();
        }

        public void OnDrop(PointerEventData EventData)
        {
            dropedContainer = sendNumber;
        }

        private void SetContainerNumber()
        {
            if(number == -1) number = int.Parse(gameObject.name.Substring(gameObject.name.Length - 1));
            controller = new ContainerController(number);

            if (number < 2) sendNumber = number;
            else sendNumber = 2;

            containers[sendNumber] = this;
        }

        protected int LoadId(int slot)
        {
            return slots[slot].Item.id;
        }

        //아이템 획득
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
            if (slot is -1) return;
            
            if(controller.container.slots.ContainsKey(slot))
                slots[slot].UpdateSlot(controller.container.slots[slot]);
            else slots[slot].UpdateSlot(ItemDummyData.NullItem);
        }
        
    }
}