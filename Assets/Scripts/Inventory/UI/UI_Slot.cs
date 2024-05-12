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
public class UI_Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Button subBtn;
    public Toggle slotBtn { get; private set; }
    public Action<int> OnDoubleClick = null;
    public Action<int> DropItem = null;
    private TextMeshProUGUI countTxt;
    private Vector3 originPosition;
    private bool isDraging = false;
    private bool isNull = true;

    private Image image;

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
        image = Util.FindChild<Image>(gameObject , "BaseImage");
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
        
    //슬롯 초기화
    private void ClearSlot()
    {
        HideBtn();
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDraging = true;
        originPosition = transform.position;
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isNull) return;
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag" + transform.position + " 복귀 " + originPosition);
        DropItem.Invoke(SlotNumber);
        transform.position = originPosition;
        isDraging = false;
        image.raycastTarget = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDraging)
        {
            isDraging = false;
            slotBtn.isOn = false;
        }
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
