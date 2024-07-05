using System.Collections;
using System.Collections.Generic;
using ItemContainer;
using UnityEngine;

public class UI_InvenBar_below : UI_Container
{
    //선택한 슬롯
    public int selectedSlot = 0;

    public override void Start()
    {
        base.Start();
        slots[selectedSlot].slotBtn.isOn = true;

        for (int i = 0; i < 6; i++)
        {
            int index = i;
            slots[index].slotBtn.onValueChanged.AddListener((on)=> ChangeSlot(index));
        }
    }

    private void ChangeSlot(int slotNumber)
    {
        selectedSlot = slotNumber;
    }

    //좌우 select 이동
    public void OnRight()
    {
        slots[++selectedSlot].slotBtn.isOn = true;
    }
    public void OnLeft()
    {
        slots[--selectedSlot].slotBtn.isOn = true;
    }

    // 선택한 아이템 정보 가져오기
    public ItemVO GetSelectedItem()
    {
        return slots[selectedSlot].Item;
    }
}
