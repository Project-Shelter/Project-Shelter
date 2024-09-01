using System;
using UnityEngine;
using System.Collections.Generic;
using ItemContainer;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Toggle = UnityEngine.UI.Toggle;

public class CraftPresenter : UI_Popup
{
    //종류별 정렬 버튼
    private Dictionary<ItemKind, Toggle> kindToggles = new Dictionary<ItemKind, Toggle>();
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
        InitCraftList();
        
        foreach (var list in listView)
            list.Value?.BindEventOnItem(PickCraftItem); 
        
        kindToggles[ItemKind.Weapon].onValueChanged.AddListener((onToggle => {listView[ItemKind.Weapon].gameObject.SetActive(onToggle);}));
        kindToggles[ItemKind.Tool].onValueChanged.AddListener((onToggle => {listView[ItemKind.Tool].gameObject.SetActive(onToggle);}));
        kindToggles[ItemKind.Building].onValueChanged.AddListener((onToggle => {listView[ItemKind.Building].gameObject.SetActive(onToggle);}));

        listView[ItemKind.Tool].gameObject.SetActive(false);
        listView[ItemKind.Building].gameObject.SetActive(false);
    }

    public void Start()
    {
        Init();
    }

    //리스트 초기화
    private void InitCraftList()
    {
        foreach (var list in listView)
        {
            list.Value?.UpdateCraftListView(model.GetCraftList(list.Key));
        }
    }

    //제작 리스트에서 아이템 선택(Button)
    public void PickCraftItem(int itemID)
    {
        currentItemID = itemID;
        craftView.UpdateCraftItem(new CraftItem(ItemDummyData.ItemDB.data[itemID]), model.MaterialsIcon(itemID));
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
        
        listView.Add(ItemKind.Weapon, 
            Util.FindChild<UI_Listing>(gameObject, ItemKind.Weapon+"CraftList", true));
        listView.Add(ItemKind.Tool, 
            Util.FindChild<UI_Listing>(gameObject, ItemKind.Tool+"CraftList", true));
        listView.Add(ItemKind.Building, 
            Util.FindChild<UI_Listing>(gameObject, ItemKind.Building+"CraftList", true));

        string[] kinds = { "Weapon", "Tool", "Building" };
        
        //UI 인풋 바인딩
        Bind<Toggle>(typeof(ItemKind));
        Bind<Button>(typeof(Buttons));
        
        kindToggles.Add(ItemKind.Weapon, Get<Toggle>((int)ItemKind.Weapon)); 
        kindToggles.Add(ItemKind.Tool, Get<Toggle>((int)ItemKind.Tool)); 
        kindToggles.Add(ItemKind.Building, Get<Toggle>((int)ItemKind.Building)); 
        
        craftButton = GetButton((int)Buttons.CraftButton);
    }
}
