using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

//추후 보관함을 만들 때 아이템을 저장하는 행위 + 데이터 자체를 하나의 부모 클래스로 만들어도 좋을 듯.
public class Inventory : MonoSingleton<Inventory>
{
    public InvenDB DB = new InvenDB();
    
    public Action<int> UpdateSlot = null;
    public Action<int> UpdateQuickSlot = null;
    public Action InitQuickBar = null; //너무귀찮아서... 씬에 있는 인베 ㄴ바... 업데이트
    public Action<int> OpenChest = null;
    
    /*
     * GET
     * 1. int 현재 최대 무게
     * 2. int 현재 아이템들의 총 무게
     * 3. item id로 아이템 정보 얻어오기
     * 4. bool 현재 이 아이템을 갖고 있는지
     * 5. 최초 InvenItems의 빈 공간 인덱스 반환 #
     * 6. 최초 QuickItems의 빈 공간 인덱스 반환 #
     */
    
    //[BE] GET
    public int currentMaxWeight { get { return DB.defaultMaxWeight + DB.currentBagExtension; }}
    public int currentWeight { get { return DB.currentWeight; } private set { DB.currentWeight = value; } }
    public Item getItemById(string id) { return DB.ItemDB[id]; }
    public bool hasItem(string id) { return DB.Items.ContainsKey(id); }

    public int emptyInvenSlot
    {
        get
        {
            for (int i = 0; i < DB.InvenItems.Length; i++)
                if (DB.InvenItems[i].name == null)
                    return i;
            return -1;
        }
    }
    public int emptyQuickSlot
    {
        get
        {
            for (int i = 0; i < DB.QuickItems.Length; i++)
                if (DB.QuickItems[i].name == null)
                    return i;
            return -1;
        }
    }

    public InvenItem getInvenItemBySlot(int slot) { return DB.InvenItems[slot];}

    public Item getItemBySlot(int slot)
    {
        if (DB.InvenItems[slot].name == null) return null;
        return DB.ItemDB[DB.InvenItems[slot].name];
    }
    
    public InvenItem getQuickInvenItemBySlot(int slot) { return DB.QuickItems[slot]; }

    public Item getQuickItemBySlot(int slot)
    {
        if (DB.QuickItems[slot].name == null) return null;
        return DB.ItemDB[DB.QuickItems[slot].name];
    }
    

    /*
     * POST
     * 1. POST {아이템}을 먹었다!
     * 2. POST {아이템}을 퀵슬롯에 장착했다!
     * 3. POST {아이템}을 사용했다! #
     * 4. POST {아이템}을 버렸다! (#어느 슬롯 아이템 버릴지 구현 X)
     */
    
    //[BE] POST
    
    //아이템 추가
    public void AddItem(string item, int count = 1)
    {
        if (DB.ItemDB[item].weight * count + currentWeight > currentMaxWeight || count > DB.ItemDB[item].overlapCount)
        {
            //필요하다면 이 경우에도 Action 추가
            return;
        }
        if (!hasItem(item))
        {
            DB.Items.Add(item, 0);
        }

        DB.Items[item] += count;
        currentWeight += DB.ItemDB[item].weight * count;

        //수정된 슬롯 반환 -> [FE] 수정한 슬롯 업데이트 (슬롯 번호를 받는 업데이트 함수 만들기)
        
        //만약 동일 아이템에 overlap 수까지 안 찬 게 있다면 그걸 먼저 채운다.
        //채우고도 count가 0이 아니면 overlap 수에 맞춰 새 슬롯에 넣는다.

        for (int i = 0; i < DB.InvenItems.Length; i++)
        {
            InvenItem _item = getInvenItemBySlot(i); //ref?
            if (_item.name == null)
                continue;
            
            int overlapCount = DB.ItemDB[item].overlapCount;
            
            if (_item.name == item && _item.count < overlapCount)
            {
                int margin = overlapCount - _item.count;
                
                _item.count += (margin > count) ? count : margin;

                count -= margin;

                DB.InvenItems[i].count = _item.count;
                
                UpdateSlot?.Invoke(i);

            }
            if (count <= 0)
                break;
        }

        while (count > 0)
        {
            int slot = emptyInvenSlot;
            if (slot == -1)
                return;
            
            int overlapCount = DB.ItemDB[item].overlapCount;
            
            int value = (overlapCount > count) ? count : overlapCount;

            DB.InvenItems[slot] = new InvenItem(item, value); //overlap 처리..

            count -= value;
            
            UpdateSlot?.Invoke(slot);
        }
        
        return;
    }

    //아이템 장착
    public int EquipItem(int slot)
    {
        //퀵슬롯에 아이템 삽입 및 삽입한 슬롯 반환 -> [FE]에서 수정한 슬롯 업데이트
        int quickSlot = emptyQuickSlot;
        DB.QuickItems[quickSlot] = DB.InvenItems[slot];
            
        //InvenItems에서 아이템 삭제  -> [FE]에서 수정한 슬롯 업데이트
        ClearSlot(slot);
        
        Inventory.Instance.InitQuickBar.Invoke();
        
        return quickSlot;
    }

    //슬롯 비우기
    private void ClearSlot(int slot)
    {
        DB.InvenItems[slot] = new InvenItem();
    }

    private void ClearQuickSlot(int slot)
    {
        DB.QuickItems[slot] = new InvenItem();
    }

    //사용
    public void UseItem(int slot, int count = 1)
    {
        DB.ItemDB[DB.QuickItems[slot].name].UseItem?.Invoke();
        RemoveQuickItem(slot, 1);
    }

    //버리기
    public void RemoveItem(int slot)
    {
        InvenItem item = DB.InvenItems[slot];
        
        if (!hasItem(item.name))
        {
            Debug.Log("@ERROR@ Somehow player is removing item in odd way!");
            return;
        }

        DB.Items[item.name] -= item.count;
        DB.currentWeight -= DB.ItemDB[item.name].weight * item.count;
        
        ClearSlot(slot);
        
        UpdateSlot?.Invoke(slot);
    }
    
    //위험한 코드.. 수정하기(
    public void RemoveItem(int slot, int count)
    {
        InvenItem item = DB.InvenItems[slot];
        
        if (!hasItem(item.name))
        {
            Debug.Log("@ERROR@ Somehow player is removing item in odd way!");
            return;
        }

        DB.Items[item.name] -= count;
        DB.InvenItems[slot].count -= count;
        DB.currentWeight -= DB.ItemDB[item.name].weight * count;
        
        if(DB.InvenItems[slot].count <= 0)
            ClearSlot(slot);
        
        UpdateSlot?.Invoke(slot);
    }

    public void RemoveQuickItem(int slot, int count = 1)
    {
        InvenItem item = DB.QuickItems[slot];

        if (item.name == null) 
        {
            return;
        }
        
        if (!hasItem(item.name))
        {
            Debug.Log("@ERROR@ Somehow player is removing item in odd way!");
            return;
        }
        
        DB.Items[item.name] -= count;
        DB.QuickItems[slot].count -= count;
        DB.currentWeight -= DB.ItemDB[item.name].weight * count;
        
        if(DB.QuickItems[slot].count <= 0)
            ClearQuickSlot(slot);
        
        InitQuickBar.Invoke();
        UpdateQuickSlot?.Invoke(slot);
    }
}
