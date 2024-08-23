using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//Craft 제작 리스트 - 아이템 리스트 View
public class UI_Listing : MonoBehaviour
{
    private Dictionary<int, UI_CraftItem> ItemList = new Dictionary<int, UI_CraftItem>();

    public void UpdateCraftListView(List<CraftItem> data)
    {
        foreach (var item in data)
        {
            UI_CraftItem instance = Managers.Resources.Instantiate("Prefabs/UI/Subitem/CraftItem", transform)
                .GetComponent<UI_CraftItem>();
            instance.UpdateView(item);
            ItemList.Add(GetInstanceID(), instance);
        }
    }

    //의존성이 너무 높음
    public void BindEventOnItem(Action<int> action)
    {
        foreach (var item in ItemList)
        {
            item.Value.BindEvent(delegate { action(item.Key);});
        }
    }
}

