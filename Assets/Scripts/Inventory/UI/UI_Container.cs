using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ItemContainer
{
    public class UI_Container : UI_Popup
    {
        public int containerID { get; protected set; } = -1;
        public int maxCapacity { get; protected set; }
        public ContainerModel Model = null;
        public UI_Slot[] slots { get; protected set; }

        public void SetContainerToStart(ContainerModel model)
        {
            Model = model;
            maxCapacity = model.container.maxCapacity;
            slots = new UI_Slot[maxCapacity];
            Init();
            InitView();

            Model.AddItemAction -= InitView;
            Model.AddItemAction += InitView;
            Model.RemoveItemAction -= InitView;
            Model.RemoveItemAction += InitView;
        }

        //기존 start. Inventory System에서만 사용하기 위해 분리.
        protected void StartContainer()
        {
            if (containerID == -1) containerID = int.Parse(gameObject.name.Substring(gameObject.name.Length - 1));
            Model ??= ContainerInjector.InjectContainer(containerID);
            
            maxCapacity = ItemDummyData.MaxCapacity[containerID];
            slots = new UI_Slot[maxCapacity];
            Init();
            InitView();

            Model.AddItemAction -= InitView;
            Model.AddItemAction += InitView;
            Model.RemoveItemAction -= InitView;
            Model.RemoveItemAction += InitView;
        }

        //모든 init이 여기에서 일어나지 않음 주의.
        protected void InitContainer()
        {
            if (containerID == -1) containerID = int.Parse(gameObject.name.Substring(gameObject.name.Length - 1));
            Model = ContainerInjector.InjectContainer(containerID);
        }
        public virtual void OnEnable()
        {
            OpenInventory();
        }
        public virtual void OnDisable()
        {
            CloseInventory();
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
        public void InitView()
        {
            for (int i = 0; i < maxCapacity; i++)
            {
                UpdateSlot(i);
            }
        }
        
        protected void OpenInventory()
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