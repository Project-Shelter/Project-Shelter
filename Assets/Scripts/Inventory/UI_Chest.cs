using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ItemContainer
{
    public class UI_Chest : UI_Container, IDropHandler
    {
        private static int chestNumber;
        public void GiveAll(int receiver)
        {
            for (int i = 0; i < maxCapacity; i++)
            {
                if (slots[i].Item.id is not 0)
                {
                    GiveItem(controller.container.slots[i].Count, i, receiver);
                }
            }
        }

        public static void ChangeChest(int number)
        {
            chestNumber = number;
        }
        
        public override void Start()
        {
            number = chestNumber;
            base.Start();
        }

        public override void OnEnable()
        {
            number = chestNumber;
            base.OnEnable();
        }

        public override void Init()
        {
            base.Init();
            
            for (int i = 0; i < maxCapacity; i++)
            {
                int slot = i;
                slots[slot].OnDoubleClick -= SlotDoubleClick;
                slots[slot].OnDoubleClick += SlotDoubleClick;
            }
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log("Chest");
            dropedContainer = 2;
        }
        
        private void SlotDoubleClick(int slot)
        {
            GiveItem(controller.container.slots[slot].Count, slot, 0);
        }
    }
}
