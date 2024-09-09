using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//Craft 제작 리스트 - 아이템 리스트 View
public class UI_Listing : MonoBehaviour
{
    //리스트 아이템 ID - UI 오브젝트
    private Dictionary<int, UI_CraftItem> ItemList = new Dictionary<int, UI_CraftItem>();
    private Transform content;

    void Awake()
    {
        content = Util.FindChild<Transform>(gameObject, "Content", true);
    }
    public void UpdateCraftListView(List<CraftItem> data)
    {
        foreach (var item in data)
        {
            UI_CraftItem instance = Managers.Resources.Instantiate("UI/Subitem/CraftItem", content)
                .GetComponent<UI_CraftItem>();
            instance.UpdateView(item);
            ItemList.Add(item.ID, instance);
        }
    }

    //의존성이 너무 높음
    public void BindEventOnItem(Action<int> action)
    {
        foreach (var item in ItemList)
        {
            item.Value.BindEvent(action);
        }
    }
}

