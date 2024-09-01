using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupCraft : MonoBehaviour
{
    private UI_Popup popup;

    public void Awake()
    {
        GameObject root = Util.FindChild(Managers.UI.Root, "Craft", true);
        popup = root.transform.GetChild(0).GetComponent<UI_Popup>();
    }
        
    public void Open()
    {
        Managers.UI.EnablePopupUI(popup);
    }
        
    public void Close()
    {
        Managers.UI.DisablePopupUI(popup);
    }
}
