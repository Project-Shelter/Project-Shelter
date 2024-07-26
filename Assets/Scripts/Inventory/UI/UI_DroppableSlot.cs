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
public class UI_DroppableSlot : UI_Slot, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Button subBtn;
    public Action<int> DropItem = null;
    private Vector3 originPosition;
    private bool isDraging = false;

    private Image image;
    

    
    void Awake()
    {
        base.Awake();
        subBtn = Util.FindChild<Button>(gameObject , "subBtn");
        image = Util.FindChild<Image>(gameObject , "BaseImage");
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
        DropItem.Invoke(SlotNumber);
        transform.position = originPosition;
        isDraging = false;
        image.raycastTarget = true;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (isDraging)
        {
            isDraging = false;
            slotBtn.isOn = false;
        }
        base.OnPointerClick(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(UI_InvenSystem.openContainer[2]) ShowBtn();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        HideBtn();
    }

    protected void ShowBtn()
    {
        if (subBtn is null)
            return;
        
        if(slotBtn.image.sprite.name != "plain")
            subBtn.gameObject.SetActive(true);
    }
    protected void HideBtn()
    {
        if (subBtn is null)
            return;
        
        if(slotBtn.image.sprite.name != "plain")
            subBtn.gameObject.SetActive(false);
    }

    public override void ClearSlot()
    {
        HideBtn();
        base.ClearSlot();
    }
}

}
