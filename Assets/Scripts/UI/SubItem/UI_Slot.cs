using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button subBtn;
    private Toggle slotBtn;
    void Start()
    {
        subBtn = Util.FindChild<Button>(gameObject , "subBtn");
        if(subBtn != null) //이게 왜 null 이 나는지... 원인규명불가.
            subBtn.onClick.AddListener(ClickSub);
        slotBtn = GetComponent<Toggle>();
    }

    private void ClickSub()
    {
        
        int slot = slotBtn.name[slotBtn.name.Length - 1] - '0' - 1;
        
        if (Inventory.Instance.getInvenItemBySlot(slot).count - 1 == 0)
            HideBtn();
        
        UI_Chest.chest.AddItem(Inventory.Instance.getInvenItemBySlot(slot).name, 1);
        Inventory.Instance.RemoveItem(slot, 1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowBtn();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideBtn();
    }

    private void ShowBtn()
    {
        if (subBtn == null)
            return;
        
        if(slotBtn.image.sprite.name != "plain")
            subBtn.gameObject.SetActive(true);
    }

    private void HideBtn()
    {
        if (subBtn == null)
            return;
        
        if(slotBtn.image.sprite.name != "plain")
            subBtn.gameObject.SetActive(false);
    }
}
