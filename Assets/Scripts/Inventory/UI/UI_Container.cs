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
        public ContainerController controller { get; protected set; }
        public UI_Slot[] slots { get; protected set; }

        public virtual void Start()
        {
            InitContainer();
            //Chest에서 여기서 오류날 가능성 존재
            maxCapacity = ItemDummyData.MaxCapacity[containerID];
            slots = new UI_Slot[maxCapacity];
            Init();
            InitView();

            controller.AddItemAction -= InitView;
            controller.AddItemAction += InitView;
            controller.RemoveItemAction -= InitView;
            controller.RemoveItemAction += InitView;
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
            controller = ContainerInjector.InjectContainer(containerID);
            Debug.Log($"Set Container : {containerID}");
        }

        public override void Init()
        {
            base.Init();
            
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
            controller?.SetContainer(containerID);
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
        
        protected void UpdateSlot(int slot)
        {
            if (slot is -1) return;
            if(controller.container.slots.ContainsKey(slot))
                slots[slot].UpdateSlot(controller.container.slots[slot]);
            else slots[slot].UpdateSlot(ItemDummyData.NullItem);
        }
        
    }
}