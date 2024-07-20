using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ItemContainer
{
    public class UI_Container : UI_Section
    {
        public int containerID { get; protected set; } = -1;
        public int maxCapacity { get; protected set; }
        public ContainerModel Model { get; protected set; }
        public UI_Slot[] slots { get; protected set; }

        public virtual void Start()
        {
            InitContainer();
            maxCapacity = ItemDummyData.MaxCapacity[containerID];
            slots = new UI_Slot[maxCapacity];
            Init();
            InitView();

            Model.AddItemAction -= InitView;
            Model.AddItemAction += InitView;
            Model.RemoveItemAction -= InitView;
            Model.RemoveItemAction += InitView;
        }
        public virtual void OnEnable()
        {
            OpenInventory();
        }
        public virtual void OnDisable()
        {
            CloseInventory();
        }

        protected void InitContainer()
        {
            if (containerID == -1) containerID = int.Parse(gameObject.name.Substring(gameObject.name.Length - 1));
            Model = ContainerInjector.InjectContainer(containerID);
        }

        public override void Init()
        {
            base.Init();
            //Connect Slot - Object
            
            //버튼 Bind - Obj name : item_{i}
            string[] slotStr = new string[maxCapacity];
            for (int i = 0; i < maxCapacity; i++)
            {
                if (i / 10 >= 1)
                    slotStr[i] = $"panelItem_{i}";
                else slotStr[i] = $"panelItem_0{i}";
            }
            
            Bind<UI_Slot>(slotStr);

            for (int i = 0; i < maxCapacity; i++)
            {
                slots[i] = Get<UI_Slot>(i);
            }
        }

        protected int LoadId(int slot)
        {
            return slots[slot].Item.id;
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
            Model?.SetContainer(containerID);
            InitView();
        }
        
        //인벤토리 닫기
        private void CloseInventory()
        {
            FlushItem();
        }

        //선택된 아이템 Flush
        protected void FlushItem()
        {
            for (int i = 0; i < maxCapacity; i++) { slots[i].TurnOff(); }
        }
        
        //Slot 출력(DB - View 반영)
        protected void UpdateSlot(int slot)
        {
            if (slot is -1) return;
            if(Model.container.slots.ContainsKey(slot))
                slots[slot].UpdateSlot(Model.container.slots[slot]);
            else slots[slot].UpdateSlot(ItemDummyData.NullItem);
        }
        
    }
}