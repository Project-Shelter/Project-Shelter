using UnityEngine;

namespace ItemContainer
{
    public class PopupTrade : MonoBehaviour
    {
        private UI_Popup popup;

        public void Awake()
        {
            GameObject root = Util.FindChild(Managers.UI.Root, "Trade", true);
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
}