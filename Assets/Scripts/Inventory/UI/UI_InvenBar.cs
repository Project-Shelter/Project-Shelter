using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ItemContainer
{
    public class UI_InvenBar : UI_InvenSystem
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

        private void SlotDoubleClick(int slot)
        {
            GiveItem(Model.container.slots[slot].Count, slot, 0);
        }
    }
}
