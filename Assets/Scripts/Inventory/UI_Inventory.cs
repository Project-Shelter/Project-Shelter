using System;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ItemContainer
{
    public class UI_Inventory : UI_Container
    {
        enum Texts
        {
            itemName,
            itemComment,
            itemWeight,
        }
        
        private TextMeshProUGUI name;
        private TextMeshProUGUI weight;
        private TextMeshProUGUI comment;

        public override void Init()
        {
            base.Init();
            Bind<TextMeshProUGUI>(typeof(Texts));
            
            name = GetText((int)Texts.itemName);
            comment = GetText((int)Texts.itemComment);
            weight = GetText((int)Texts.itemWeight);
            
            for (int i = 0; i < maxCapacity; i++)
            {
                int slot = i;
                slots[slot].slotBtn.onValueChanged.AddListener(delegate { ClickItem(slot); });
            }

            LoadWeight();
        }
        
        public void ClickItem(int slot)
        {
            LoadData(slot);
        }

        private void LoadData(int slot)
        {
            int id = LoadId(slot);
            if (id is 0)
                return;
            
            name.text = ItemDummyData.ItemDB.data[id].name;
            comment.text = ItemDummyData.ItemDB.data[id].comment;
        }
        
        public void LoadWeight()
        {
            weight.text = $"{ItemDummyData.currentWeight}/{ItemDummyData.currentMaxWeight}";
        }
    }
}