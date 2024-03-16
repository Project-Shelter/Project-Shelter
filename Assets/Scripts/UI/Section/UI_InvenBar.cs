using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InvenBar : UI_Section
{
    private const int quickSlotCount = 6;
    enum Items
    {
        item1 = 0,
        item2,
        item3,
        item4,
        item5,
        item6,
    }

    enum Texts
    {
        barItemCount_1,
        barItemCount_2,
        barItemCount_3,
        barItemCount_4,
        barItemCount_5,
        barItemCount_6,
    }

    private Toggle[] quickSlots = new Toggle[quickSlotCount];
    private TextMeshProUGUI[] quickItemCounts = new TextMeshProUGUI[quickSlotCount];
    void Start()
    {
        base.Init();

        Bind<Toggle>(typeof(Items));
        Bind<TextMeshProUGUI>(typeof(Texts));
        
        for (int i = 0; i < quickSlotCount; i++)
        {
            quickSlots[i] = Get<Toggle>(i);
            quickItemCounts[i] = GetText(i);
        }
        
        InitInvenBar();

        Inventory.Instance.InitQuickBar -= InitInvenBar;
        Inventory.Instance.InitQuickBar += InitInvenBar;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.Instance.DB.QuickItems[0].name != null)
        {
            Inventory.Instance.UseItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && Inventory.Instance.DB.QuickItems[1].name != null)
        {
            Inventory.Instance.UseItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && Inventory.Instance.DB.QuickItems[2].name != null)
        {           
            Inventory.Instance.UseItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && Inventory.Instance.DB.QuickItems[3].name != null)
        {           
            Inventory.Instance.UseItem(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && Inventory.Instance.DB.QuickItems[4].name != null)
        {          
            Inventory.Instance.UseItem(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) && Inventory.Instance.DB.QuickItems[5].name != null)
        {         
            Inventory.Instance.UseItem(5);
        }

    }
    
    public void InitInvenBar()
    {
        for (int i = 0; i < quickSlotCount; i++)
        {
            UpdateQuickSlot(i);
        }
    }
    
    //BE에서 퀵슬롯 정보를 받아 슬롯에 출력한다.
    private void UpdateQuickSlot(int slot)
    {        
        if (Inventory.Instance.getQuickInvenItemBySlot(slot).name == null || Inventory.Instance.getQuickInvenItemBySlot(slot).count == 0)
        {
            quickSlots[slot].image.sprite = Managers.Resources.Load<Sprite>("Arts/Items/plain");
            quickItemCounts[slot].text = "";
        }
        else
        {
            quickSlots[slot].image.sprite = Inventory.Instance.getQuickItemBySlot(slot).icon;
            quickItemCounts[slot].text = Inventory.Instance.getQuickInvenItemBySlot(slot).count.ToString();
        }
    }
}
