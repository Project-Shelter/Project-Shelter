using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_Chest : UI_Section
{
    private const int slotCount = 12;
    public static Chest chest;
    
    enum Buttons
    {
        takeBtn,
        takeAllBtn
    }
    enum Items
    {
        chestItem_1 = 0,
        chestItem_2,
        chestItem_3,
        chestItem_4,
        chestItem_5,
        chestItem_6,
        chestItem_7,
        chestItem_8,
        chestItem_9,
        chestItem_10,
        chestItem_11,
        chestItem_12,
    }

    enum Texts
    {
        chestItemCount_1 = 0,
        chestItemCount_2,
        chestItemCount_3,
        chestItemCount_4,
        chestItemCount_5,
        chestItemCount_6,
        chestItemCount_7,
        chestItemCount_8,
        chestItemCount_9,
        chestItemCount_10,
        chestItemCount_11,
        chestItemCount_12,
    }
    
    private Button takeButton;
    private Button takeAllButton;
    
    private TextMeshProUGUI[] itemCounts = new TextMeshProUGUI[slotCount];
    private Toggle[] slots = new Toggle[slotCount];
    private List<int> pickedItem = new List<int>();
    
    void Start()
    {
        Init();
        gameObject.SetActive(false);
    }

    //키보드 입력 임시처리 - 추후 인풋 시스템 갈아엎을거라...
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClickExitBtn();
        }
    }

    public override void Init()
    {
        base.Init();

        if(chest == null)
        {
            chest = ItemDummyData.chests[0];
        }

        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<Toggle>(typeof(Items));

        takeButton = GetButton((int)Buttons.takeBtn);
        takeAllButton = GetButton((int)Buttons.takeAllBtn);

        for (int i = 0; i < slotCount; i++)
        {
            slots[i] = Get<Toggle>(i);
            itemCounts[i] = GetText(i);
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

        #endregion
        
        takeButton.onClick.AddListener(ClickTakeBtn);
        takeAllButton.onClick.AddListener(ClickTakeAllBtn);

        InitInven();
        
        chest.UpdateSlot -= UpdateSlot;
        chest.UpdateSlot += UpdateSlot;

        Inventory.Instance.OpenChest -= ChangeChest;
        Inventory.Instance.OpenChest += ChangeChest;

    }

    private void ChangeChest(int chestNum)
    {
        chest = ItemDummyData.chests[chestNum];
        InitInven();
    }

    //인벤토리 초기화 - BE 불러오기
    private void InitInven()
    {
        for (int i = 0; i < slotCount; i++)
        {
            UpdateSlot(i);
        }
    }
    
    //해당 아이템에 대한 설명을 띄운 뒤 아이템의 pick 상태를 바꾼다
    private void ClickItem(int slot)
    {
        Item item = chest.getItemBySlot(slot);
        if (item == null)
        {
            slots[slot].isOn = false;
            return;
        }
        
        
        if (slots[slot].isOn) { PickItem(slot); }
        else { UnpickItem(slot); }
    }
    private void ClickTakeBtn()
    {
        foreach (var item in pickedItem)
        {
            InvenItem invenItem = GetInvenItemBySlot(item);
            Inventory.Instance.AddItem(invenItem.name, invenItem.count);
            chest.RemoveItem(item);
        }

        int count = pickedItem.Count;
        for (int i = 0; i < count; i++)
        {
            ClearSlot(pickedItem[0]);
        }
    }
    private void ClickTakeAllBtn()
    {
        for(int i = 0; i < slotCount; i++)
        {
            InvenItem invenItem = GetInvenItemBySlot(i);
            if (invenItem.name == null)
                continue;
            Inventory.Instance.AddItem(invenItem.name, invenItem.count);
            chest.RemoveItem(i);
            ClearSlot(i);
        }
    }
    
    private void ClickExitBtn()
    {
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
    }
    
    //아이템을 pick한다.
    private void PickItem(int slot)
    {
        Debug.Log("pick " + slot);

        pickedItem.Add(slot);
    }

    private Item GetItemBySlot(int slot)
    {
        return chest.getItemBySlot(slot);
    }
    private InvenItem GetInvenItemBySlot(int slot)
    {
        return chest.getInvenItemBySlot(slot);
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
    #endregion
}
