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
            slots[index].slotBtn.onValueChanged.AddListener((on)=> ChangeSlot(index));
        }
        slots[selectedSlot].slotBtn.isOn = true;


        ServiceLocator.GetService<ActorController>().SwitchActorAction += () =>
        {
            ServiceLocator.GetService<ActorController>().CurrentActor.SetItem(GetSelectedItem());
        };
    }

    private void ChangeSlot(int slotNumber)
    {
        if(selectedSlot == slotNumber) return;
        selectedSlot = slotNumber;
        ServiceLocator.GetService<ActorController>().CurrentActor.SetItem(GetSelectedItem());
    }

    //좌우 select 이동
    public void OnRight()
    {
        Debug.Log("RIght");
        if (selectedSlot + 1 >= 6) return;
        slots[++selectedSlot].slotBtn.isOn = true;
    }
    public void OnLeft()
    {
        if (selectedSlot - 1 < 0) return;
        slots[--selectedSlot].slotBtn.isOn = true;
    }

    public void OnFirst()
    {
        
    }

    public void OnSecond()
    {
        
    }

    public void OnThird()
    {
        
    }

    public void OnFourth()
    {
        
    }

    public void OnFifth()
    {
        Debug.Log("Fifth");
    }

    public void OnSixth()
    {
        
    }

    // 선택한 아이템 정보 가져오기
    public ItemVO GetSelectedItem()
    {
        return slots[selectedSlot].Item;
    }
}
