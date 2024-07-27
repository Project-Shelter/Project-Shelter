using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ItemContainer
{
    public class UI_Trade : MonoBehaviour
    {
        private UI_Container itemTable;
        private UI_Container tradeTable;
        public Button tradeButton;
        public UI_Container otherTradeTable;
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
                itemTableNumber = DayNight.Instance.dayCount + 700;
                Debug.Log(itemTableNumber);
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
            UpdateTradeButton();
        }
        
        //trade -> item
        public void MoveToItem(int slot)
        {
            int moveItemID = tradeTable.slots[slot].Item.id;
            if (moveItemID is 0) return;
            itemTable.Model.AddItem(tradeTable.slots[slot].Item.id, 1);
            tradeTable.Model.RemoveItem(slot, 1);
            UpdateTradeButton();
        }

        //Trade 버튼 클릭
        public void Trade()
        {
            //아이템 이동
            foreach (var item in otherTradeTable.Model.container.slots)
            {
                itemTable.Model.AddItem(item.Value.id, item.Value.Count);
            }
            //이동 후 UI TradeTable 비우기
            otherTradeTable.Model.container.slots.Clear();
            //테이블 변화 UI 반영
            otherTradeTable.InitView();
            itemTable.InitView();

            //플레이어일 시, 인벤토리(DB)에 반영
            if (trader == 0)
            {
                ItemDummyData.invenSlots[0] = itemTable.Model.container.slots;
            }
        }

        public bool CanTrade()
        {
            int playerValue = 0;
            int otherValue = 0;
            foreach (var item in tradeTable.Model.container.slots)
            {
                playerValue += ItemDummyData.ItemDB.data[item.Value.id].weight * item.Value.Count;
            }
            foreach (var item in otherTradeTable.Model.container.slots)
            {
                otherValue += ItemDummyData.ItemDB.data[item.Value.id].weight * item.Value.Count;
            }

            return playerValue >= otherValue;
        }

        public void UpdateTradeButton()
        {
            if (CanTrade())
            {
                tradeButton.interactable = true;
            }
            else
            {
                tradeButton.interactable = false;
            }
        }
    }
}