using System.Collections.Generic;
using UnityEngine;

namespace ItemContainer
{
    public class UI_Trade : MonoBehaviour
    {
        private UI_Container itemTable;
        private UI_Container tradeTable;
        private int itemTableNumber = 0;
        public int trader { get; private set; }= 0; //0 - me, 1 - you
        
        public void Awake()
        {
            itemTable = transform.GetChild(0).GetComponent<UI_Container>();
            tradeTable = transform.GetChild(1).GetComponent<UI_Container>();
            //현재 invenSlots에서 하드코딩으로 가져오고 있는데 Day 따라서 가져오는 것으로 수정할 것.
            //-> 거래 시스템이 Day에 종속되어 있음.
            if(gameObject.name.Substring(gameObject.name.Length - 3) == "YOU")
            {
                trader = 1;
                //itemTableNumber = 날짜
            }
        }

        public void Start()
        {
            itemTable.SetContainerToStart(new ContainerModel(SetInventory(), ItemDummyData.MaxCapacity[0]));
            tradeTable.SetContainerToStart(new ContainerModel(new Dictionary<int, ItemVO>(), ItemDummyData.MaxCapacity[3]));

            DoubleClick();
        }

        private Dictionary<int, ItemVO> SetInventory()
        {
            Dictionary<int, ItemVO> returnDictionary = new Dictionary<int, ItemVO>();
            foreach (var item in ItemDummyData.invenSlots[itemTableNumber])
            {
                ItemVO newItem = new ItemVO(item.Value.id, item.Value.Count);
                returnDictionary.Add(item.Key, newItem);
            }

            return returnDictionary;
        }

        public void DoubleClick()
        {
            foreach(var slot in itemTable.slots)
            {
                slot.OnDoubleClick -= MoveToTrade;
                slot.OnDoubleClick += MoveToTrade;
            }

            foreach (var slot in tradeTable.slots)
            {
                slot.OnDoubleClick -= MoveToItem;
                slot.OnDoubleClick += MoveToItem;
            }
        }
        
        //item -> trade
        public void MoveToTrade(int slot)
        {
            int moveItemID = itemTable.slots[slot].Item.id;
            if (moveItemID is 0) return;
            tradeTable.Model.AddItem(moveItemID, 1);
            itemTable.Model.RemoveItem(slot, 1);
        }
        
        //trade -> item
        public void MoveToItem(int slot)
        {
            int moveItemID = tradeTable.slots[slot].Item.id;
            if (moveItemID is 0) return;
            itemTable.Model.AddItem(tradeTable.slots[slot].Item.id, 1);
            tradeTable.Model.RemoveItem(slot, 1);
        }

        //Trade 버튼 클릭
        public void Trade()
        {
            if (trader == 0)
            {
                //상대방 trade를 가져와야 ...
                ItemDummyData.invenSlots[0] = itemTable.Model.container.slots;
            }
            
            tradeTable.Model.container.slots.Clear();
            itemTable.InitView();
            tradeTable.InitView();
        }
    }
}