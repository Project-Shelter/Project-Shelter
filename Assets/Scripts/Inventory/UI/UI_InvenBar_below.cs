using System.Collections;
using System.Collections.Generic;
using ItemContainer;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_InvenBar_below : UI_Container
{
    //선택한 슬롯
    public int selectedSlot = 0;

    public void Start()
    {
        base.StartContainer();

        for (int i = 0; i < 6; i++)
        {
            int index = i;
            slots[index].slotBtn.onValueChanged.AddListener((on)=> ChangeSlot(on, index));
        }
        slots[selectedSlot].slotBtn.isOn = true;


        ServiceLocator.GetService<ActorController>().SwitchActorAction += () =>
        {
            ServiceLocator.GetService<ActorController>().CurrentActor.SetItem(GetSelectedItem());
        };
    }

    private void ChangeSlot(bool isOn, int slotNumber)
    {
        if (!isOn) return;
        selectedSlot = slotNumber;
        ServiceLocator.GetService<ActorController>().CurrentActor.SetItem(GetSelectedItem());
    }

    //좌우 select 이동
    public void OnRight()
    {
        if (selectedSlot + 1 >= 6) return;
        if(slots[selectedSlot+1].Item.id == 0) return;
        slots[++selectedSlot].slotBtn.isOn = true;
    }
    public void OnLeft()
    {
        if (selectedSlot - 1 < 0) return;
        if (slots[selectedSlot - 1].Item.id == 0) return;
        slots[--selectedSlot].slotBtn.isOn = true;
    }

    public void OnFirst()
    {
        if (slots[0].Item.id == 0) return;
        slots[0].slotBtn.isOn = true;
    }

    public void OnSecond()
    {
        if (slots[1].Item.id == 0) return;
        slots[1].slotBtn.isOn = true;
    }

    public void OnThird()
    {
        if (slots[2].Item.id == 0) return;
        slots[2].slotBtn.isOn = true;
    }

    public void OnFourth()
    {
        if (slots[3].Item.id == 0) return;
        slots[3].slotBtn.isOn = true;
    }

    public void OnFifth()
    {
        if (slots[4].Item.id == 0) return;
        slots[4].slotBtn.isOn = true;
    }

    public void OnSixth()
    {
        if (slots[5].Item.id == 0) return;
        slots[5].slotBtn.isOn = true;
    }

    // 선택한 아이템 정보 가져오기
    public ItemVO GetSelectedItem()
    {
        return slots[selectedSlot].Item;
    }
}
