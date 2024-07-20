using System;
using System.ComponentModel;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ItemContainer
{
    public class UI_Inventory : UI_InvenSystem
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

        public GameObject btn;
        public GameObject btnFarming;
        public GameObject btnFarmingClick;

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
                slots[slot].OnDoubleClick -= SlotDoubleClick;
                slots[slot].OnDoubleClick += SlotDoubleClick;
            }

            LoadWeight();

            pickItem -= ClickQuickBtn;
            pickItem += ClickQuickBtn;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            
            OpenInventoryBtn();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            FlushBtn();
        }

        private void OpenInventoryBtn()
        {
            if (openContainer[2]) btnFarming.SetActive(true);
            else btn.SetActive(true);
            
        }
        private void FlushBtn()
        {
            btn.SetActive(true);
            btnFarming.SetActive(false);
            btnFarmingClick.SetActive(false);
        }

        public void ClickQuickBtn(int sender)
        {
            if (!openContainer[2])
                return;
            if(pickedContainer == 1) btnFarmingClick.SetActive(true);
            else btnFarmingClick.SetActive(false);
        }

        private void SlotDoubleClick(int slot)
        {
            GiveItem(Model.container.slots[slot].Count, slot, 1);
        }
        
        private void ClickItem(int slot)
        {
            LoadData(slot);
        }

        private void LoadData(int slot)
        {
            int id = LoadId(slot);
            if (id is 0)
                return;
            
            name.text = ItemDummyData.ItemDB.data[id].name;
            comment.text = ItemDummyData.ItemDB.data[id].description;
        }
        
        public void LoadWeight()
        {
            weight.text = $"{ItemDummyData.currentWeight}/{ItemDummyData.currentMaxWeight}";
        }
    }
}