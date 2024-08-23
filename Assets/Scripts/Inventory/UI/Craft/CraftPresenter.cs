using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Toggle = UnityEngine.UI.Toggle;

public class CraftPresenter : UI_Popup
{
    //종류별 정렬 버튼
    private Dictionary<ItemKind, Toggle> kindToggles;
    private Button craftButton;
    private int currentItemID;
    
    private CraftModel model;
    private Dictionary<ItemKind, UI_Listing> listView = new Dictionary<ItemKind, UI_Listing>();
    private UI_Crafting craftView;
    
    enum Buttons
    {
        CraftButton,
    }

    public override void Init()
    {
        base.Init();
        BindInstances();

        craftButton.onClick.AddListener(MakeCraftItem);
        foreach (var list in listView)
            list.Value.BindEventOnItem(PickCraftItem);
    }
    
    void Awake()
    {
        InitCraftList();
    }

    //리스트 초기화
    private void InitCraftList()
    {
        foreach (var list in listView)
            list.Value.UpdateCraftListView(model.GetCraftList(list.Key));
    }

    //제작 리스트에서 아이템 선택(Button)
    public void PickCraftItem(int itemID)
    {
        //currentItemID 업데이트
        //craftView.UpdateCraftItem();
        craftView.InteractCraftButton(model.CanCraftItem(currentItemID));
    }

    //아이템 제작
    public void MakeCraftItem()
    {
        model.MakeCraftItem(currentItemID);
        PickCraftItem(currentItemID);
    }

    private void BindInstances()
    {
        model = new CraftModel();
        craftView = GetComponentInChildren<UI_Crafting>();
        foreach (ItemKind kind in Enum.GetValues(typeof(ItemKind)))
        {
            listView.Add(kind, 
                Util.FindChild<UI_Listing>(gameObject, kind+"CraftList"));
        }
        
        //UI 인풋 바인딩
        Bind<Toggle>(typeof(ItemKind));
        Bind<Button>(typeof(Buttons));
        foreach (ItemKind itemKind in Enum.GetValues(typeof(ItemKind)))
        {
            kindToggles.Add(itemKind, Get<Toggle>((int)itemKind)); 
        }
        craftButton = GetButton((int)Buttons.CraftButton);
    }
}
