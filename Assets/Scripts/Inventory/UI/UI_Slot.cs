using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ItemContainer
{
    public class UI_Slot : MonoBehaviour, IPointerClickHandler
    {
        public Toggle slotBtn { get; private set; }
        private TextMeshProUGUI countTxt;
        public ItemVO Item { get; private set; } = new ItemVO();
        protected bool isNull = true;
        public bool IsOn { get { return slotBtn.isOn; } }
        public int SlotNumber { get; private set; }
        public Action<int> OnDoubleClick = null;


        public void Awake()
        {
            countTxt = Util.FindChild<TextMeshProUGUI>(gameObject, "itemCount");
            slotBtn = GetComponent<Toggle>();
        }
        
        void Start()
        {
            SlotNumber = int.Parse(gameObject.name.Substring(gameObject.name.Length - 2));
        }
        
        //BE에서 slot 정보를 가져와 출력
        public void UpdateSlot(ItemVO item)
        {
            LoadItem(item);

            if (Item.id is 0)
            {
                ClearSlot();
                return;
            }

            slotBtn.image.sprite = ItemDummyData.ItemDB.data[Item.id].image;
            countTxt.text = Item.Count.ToString();
            slotBtn.interactable = true;
            isNull = false;
        }
        
        public void TurnOff()
        {
            slotBtn.isOn = false;
        }
        
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount == 2)
            {
                OnDoubleClick?.Invoke(SlotNumber);
            }
        }
        
        //슬롯 초기화
        public virtual void ClearSlot()
        {
            slotBtn.image.sprite = ItemDummyData.PlainImage;
            countTxt.text = "";
            slotBtn.isOn = false;
            slotBtn.interactable = false;
            isNull = true;
        }
    
        private void LoadItem(ItemVO item)
        {
            Item = item;
            countTxt.text = Item.Count.ToString();
        }
    }
}