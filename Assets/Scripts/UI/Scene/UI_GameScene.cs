using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameScene : UI_Scene
{
    enum Buttons
    {
        PopupBtn,
    }

    //Scene Panel
    private GameObject thisPanel = null;

    void Start()
    {
        //Managers.Instance.GameOverAction += () => gameObject.SetActive(false);
        //Init();
    }

    //전부 임시 구현
    public override void Init()
    {
        base.Init();

        thisPanel = this.gameObject;
        
        //바인딩
        Bind<Button>(typeof(Buttons));
        
        //이벤트 추가 - 팝업
        GetButton((int)Buttons.PopupBtn).gameObject.BindUIEvent(InstantiatePopup);
    }
    
    
    private void InstantiatePopup(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_PopupEx>("UI_PopupEx");
    }
}
