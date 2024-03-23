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
        }
        
        public void ClickItem(int slot)
        {
            //base.ClickItem(slot);
            LoadData(slot);
        }

        private void LoadData(int slot)
        {
            name.text = ItemDummyData.ItemDB.data[controller.container.slots[slot].id].name;
            comment.text = ItemDummyData.ItemDB.data[controller.container.slots[slot].id].comment;
        }
        
        public void LoadWeight()
        {
            weight.text = $"{controller.currentWeight}/{ItemDummyData.currentMaxWeight}";
        }
    }
}