using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemContainer
{
    public class UI_Chest : UI_Container
    {
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
    }
}
