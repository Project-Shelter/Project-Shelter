using System;
using System.Collections;
using System.Collections.Generic;
using ItemContainer;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ItemContainer
{
public class UI_Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Button subBtn;
    public Toggle slotBtn { get; private set; }
    public Action<int> OnDoubleClick = null;
    private TextMeshProUGUI countTxt;

    public int SlotNumber { get; private set; }
    public ItemVO Item { get; private set; } = new ItemVO();

    public bool IsOn { get { return slotBtn.isOn; } }

    public void TurnOff()
    {
        slotBtn.isOn = false;
    }
    
    void Awake()
    {
        countTxt = Util.FindChild<TextMeshProUGUI>(gameObject, "itemCount");
        subBtn = Util.FindChild<Button>(gameObject , "subBtn");
        slotBtn = GetComponent<Toggle>();
    }

    void Start()
    {
        SlotNumber = int.Parse(gameObject.name.Substring(gameObject.name.Length - 1));
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
    }
        
    //슬롯 초기화
    private void ClearSlot()
    {
        HideBtn();
        slotBtn.image.sprite = ItemDummyData.PlainImage;
        countTxt.text = "";
        slotBtn.isOn = false;
        slotBtn.interactable = false;
    }
    
    private void LoadItem(ItemVO item)
    {
        Item = item;
        countTxt.text = Item.Count.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            OnDoubleClick?.Invoke(SlotNumber);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(UI_Container.openContainer[2]) ShowBtn();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        HideBtn();
    }

    private void ShowBtn()
    {
        if (subBtn is null)
            return;
        
        if(slotBtn.image.sprite.name != "plain")
            subBtn.gameObject.SetActive(true);
    }
    private void HideBtn()
    {
        if (subBtn is null)
            return;
        
        if(slotBtn.image.sprite.name != "plain")
            subBtn.gameObject.SetActive(false);
    }
}

}
