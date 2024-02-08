using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PopupEx : UI_Popup
{
    enum Buttons
    {
        CloseBtn
    }
    
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.CloseBtn).gameObject.BindUIEvent(ClosePopup);
    }

    private void ClosePopup(PointerEventData data)
    {
        Managers.Resources.Destroy(gameObject);
    }

}
