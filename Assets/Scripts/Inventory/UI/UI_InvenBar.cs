using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ItemContainer
{
    public class UI_InvenBar : UI_Container//, IDropHandler
    {
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
            Debug.Log("InvenBar");
            Debug.Log("OnDrop" + eventData.position);
            dropedContainer = 1;
        }

        
        private void SlotDoubleClick(int slot)
        {
            GiveItem(controller.container.slots[slot].Count, slot, 0);
        }
    }
}
