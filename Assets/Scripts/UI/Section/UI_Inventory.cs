using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Inventory : UI_Section
{
    private const int slotCount = 18;
    private const int quickSlotCount = 6;
    
    enum Buttons
    {
        throwBtn,
        equipBtn,
        exitBtn,
    }
    enum Items
    {
        panelItem_1 = 0,
        panelItem_2,
        panelItem_3,
        panelItem_4,
        panelItem_5,
        panelItem_6,
        panelItem_7,
        panelItem_8,
        panelItem_9,
        panelItem_10,
        panelItem_11,
        panelItem_12,
        panelItem_13,
        panelItem_14,
        panelItem_15,
        panelItem_16,
        panelItem_17,
        panelItem_18,
        barItem_1,
        barItem_2,
        barItem_3,
        barItem_4,
        barItem_5,
        barItem_6,

    }
    enum Texts
    {
        itemCount_1 = 0,
        itemCount_2,
        itemCount_3,
        itemCount_4,
        itemCount_5,
        itemCount_6,
        itemCount_7,
        itemCount_8,
        itemCount_9,
        itemCount_10,
        itemCount_11,
        itemCount_12,
        itemCount_13,
        itemCount_14,
        itemCount_15,
        itemCount_16,
        itemCount_17,
        itemCount_18,
        barItemCount_1,
        barItemCount_2,
        barItemCount_3,
        barItemCount_4,
        barItemCount_5,
        barItemCount_6,
        itemName,
        itemComment,
        itemWeight,
    }
    
    private Button throwButton;
    private Button equipButton;
    private Button exitButton;
    
    private Toggle[] slots = new Toggle[slotCount];
    private TextMeshProUGUI[] itemCounts = new TextMeshProUGUI[slotCount];
    
    private Toggle[] quickSlots = new Toggle[quickSlotCount];
    private TextMeshProUGUI[] quickItemCounts = new TextMeshProUGUI[quickSlotCount];
    
    private TextMeshProUGUI name;
    private TextMeshProUGUI weight;
    private TextMeshProUGUI comment;
    
    private List<int> pickedItem = new List<int>();
    
    void Start()
    {
        Init();
    }

    //키보드 입력 임시처리 - 추후 인풋 시스템 갈아엎을거라...
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
        {
            ClickExitBtn();
        }
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Toggle>(typeof(Items));
        Bind<TextMeshProUGUI>(typeof(Texts));

        throwButton = GetButton((int)Buttons.throwBtn);
        equipButton = GetButton((int)Buttons.equipBtn);
        exitButton = GetButton((int)Buttons.exitBtn);
        
        name = GetText((int)Texts.itemName);
        comment = GetText((int)Texts.itemComment);
        weight = GetText((int)Texts.itemWeight);

        for (int i = 0; i < slotCount; i++)
        {
            slots[i] = Get<Toggle>(i);
            itemCounts[i] = GetText(i);
        }

        for (int i = 0; i < quickSlotCount; i++)
        {
            quickSlots[i] = Get<Toggle>(slotCount + i);
            quickItemCounts[i] = GetText(slotCount + i);
        }
        
        #region 하드코딩
        
        slots[0].onValueChanged.AddListener(delegate { ClickItem(0); });
        slots[1].onValueChanged.AddListener(delegate { ClickItem(1); });
        slots[2].onValueChanged.AddListener(delegate { ClickItem(2); });
        slots[3].onValueChanged.AddListener(delegate { ClickItem(3); });
        slots[4].onValueChanged.AddListener(delegate { ClickItem(4); });
        slots[5].onValueChanged.AddListener(delegate { ClickItem(5); });
        slots[6].onValueChanged.AddListener(delegate { ClickItem(6); });
        slots[7].onValueChanged.AddListener(delegate { ClickItem(7); });
        slots[8].onValueChanged.AddListener(delegate { ClickItem(8); });
        slots[9].onValueChanged.AddListener(delegate { ClickItem(9); });
        slots[10].onValueChanged.AddListener(delegate { ClickItem(10); });
        slots[11].onValueChanged.AddListener(delegate { ClickItem(11); });
        slots[12].onValueChanged.AddListener(delegate { ClickItem(12); });
        slots[13].onValueChanged.AddListener(delegate { ClickItem(13); });
        slots[14].onValueChanged.AddListener(delegate { ClickItem(14); });
        slots[15].onValueChanged.AddListener(delegate { ClickItem(15); });
        slots[16].onValueChanged.AddListener(delegate { ClickItem(16); });
        slots[17].onValueChanged.AddListener(delegate { ClickItem(17); });

        #endregion
        
        throwButton.onClick.AddListener(ClickThrowBtn);
        equipButton.onClick.AddListener(ClickEquipBtn);
        exitButton.onClick.AddListener(ClickExitBtn);

        InitInven();
        Inventory.Instance.UpdateSlot -= UpdateSlot;
        Inventory.Instance.UpdateSlot += UpdateSlot;
        Inventory.Instance.UpdateQuickSlot -= UpdateQuickSlot;
        Inventory.Instance.UpdateQuickSlot += UpdateQuickSlot;
    }

    //인벤토리 초기화 - BE 불러오기
    private void InitInven()
    {
        for (int i = 0; i < slotCount; i++)
        {
            UpdateSlot(i);
        }

        for (int i = 0; i < quickSlotCount; i++)
        {
            UpdateQuickSlot(i);
        }
        
        LoadWeight();
        DisableThrowBtn();
    }
    
    //해당 아이템에 대한 설명을 띄운 뒤 아이템의 pick 상태를 바꾼다
    private void ClickItem(int slot)
    {
        Item item = Inventory.Instance.getItemBySlot(slot);
        if (item == null)
        {
            slots[slot].isOn = false;
            return;
        }
        
        LoadItemInfo(item);
        
        if (slots[slot].isOn) { PickItem(slot); }
        else { UnpickItem(slot); }
    }
    private void ClickThrowBtn()
    {
        int count = pickedItem.Count;
        for (int i = 0; i < count; i++)
        {
            Inventory.Instance.RemoveItem(pickedItem[0]);
        }

        LoadWeight();
        
        DisableThrowBtn();
    }
    private void ClickEquipBtn()
    {
        int count = pickedItem.Count;
        for (int i = 0; i < count; i++)
        {
            int updateSlot = Inventory.Instance.EquipItem(pickedItem[0]);
            UpdateQuickSlot(updateSlot);
            ClearSlot(pickedItem[0]);
        }
    }
    private void ClickExitBtn()
    {
        int count = pickedItem.Count;
        for (int i = 0; i < count; i++)
        {
            UnpickItem(pickedItem[0]);
        }
        gameObject.SetActive(false);
    }

    private void UnpickAllItem()
    {
        foreach(var item in pickedItem)
        {
            UnpickItem(item);
        }
    }

    #region  Utils

    //아이템 pick을 취소한다.
    private void UnpickItem(int slot)
    {
        pickedItem.Remove(slot);
        slots[slot].isOn = false;
        
        if (pickedItem.Count == 0) { DisableThrowBtn(); DisableEquipBtn(); }
        else { EnableThrowBtn(); }

        if (pickedItem.Count == 1) { EnableEquipBtn(); }
        else { DisableEquipBtn(); }
    }
    
    //아이템을 pick한다.
    private void PickItem(int slot)
    {
        Debug.Log("pick " + slot);

        pickedItem.Add(slot);

        if (pickedItem.Count == 0) { DisableThrowBtn(); DisableEquipBtn(); }
        else { EnableThrowBtn(); }

        if (pickedItem.Count == 1) { EnableEquipBtn(); }
        else { DisableEquipBtn(); }
    }
    
    //아이템 설명을 불러온다.
    private void LoadItemInfo(Item item)
    {
        name.text = item.name;
        comment.text = item.comment;
    }

    private void LoadWeight()
    {
        weight.text = $"{Inventory.Instance.currentWeight}/{Inventory.Instance.currentMaxWeight}";
    }
    private void EnableThrowBtn()
    {
        throwButton.interactable = true;
    }
    private void DisableThrowBtn()
    {
        throwButton.interactable = false;
    }
    
    private void EnableEquipBtn()
    {
        equipButton.interactable = true;
    }
    private void DisableEquipBtn()
    {
        equipButton.interactable = false;
    }
    private Item GetItemBySlot(int slot)
    {
        return Inventory.Instance.getItemBySlot(slot);
    }
    private InvenItem GetInvenItemBySlot(int slot)
    {
        return Inventory.Instance.getInvenItemBySlot(slot);
    }
    //슬롯에서 아이템을 삭제한다. [FE]
    private void ClearSlot(int slot)
    {
        slots[slot].image.sprite = Managers.Resources.Load<Sprite>("Arts/Items/plain");
        itemCounts[slot].text = "";
        UnpickItem(slot);
    }

    //BE에서 아이템 정보를 받아 슬롯에 출력한다.
    private void UpdateSlot(int slot)
    {
        if (GetInvenItemBySlot(slot).name == null)
        {
            ClearSlot(slot);
            return;
        }
        
        slots[slot].image.sprite = GetItemBySlot(slot).icon;
        itemCounts[slot].text = GetInvenItemBySlot(slot).count.ToString();
    }

    //BE에서 퀵슬롯 정보를 받아 슬롯에 출력한다.
    private void UpdateQuickSlot(int slot)
    {        
        if (Inventory.Instance.getQuickInvenItemBySlot(slot).name == null)
        {
            quickSlots[slot].image.sprite = Managers.Resources.Load<Sprite>("Arts/Items/plain");
            quickItemCounts[slot].text = "";
            return;
        }
        quickSlots[slot].image.sprite = Inventory.Instance.getQuickItemBySlot(slot).icon;
        quickItemCounts[slot].text = Inventory.Instance.getQuickInvenItemBySlot(slot).count.ToString();
    }

    #endregion
}
