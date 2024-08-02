using System.Collections;
using System.Collections.Generic;
using ItemContainer;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PopupContainer : MonoBehaviour
{
    protected GameObject go;
    public UI_Popup popup;
    private Button popupBtn;

    public void Awake()
    {
        popupBtn = gameObject.GetComponent<Button>();
    }
    public void Start(){
        popupBtn.onClick.AddListener(Open);
        go = popup.gameObject;
    }
    
    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(go.activeSelf) Close();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(go.activeSelf) Close();
        }
    }

    public void Open()
    {
        Managers.UI.EnablePopupUI(popup);
    }

    public void Close()
    {
        Managers.UI.DisableAllPopupUI();
    }
}
