using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

namespace ItemContainer
{
    public class UI_Chest : UI_Container
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
    }
}
