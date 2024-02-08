using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private int _order = 10;
    private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    private UI_Scene _sceneUI = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject{name = "@UI_Root"};
            
            return root;
        }
    }

    public void SetCanvas(GameObject obj, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(obj);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
        
    }

    //UI 내 요소에 서브아이템(EX. 인벤토리 내 아이템) 을 생성한다.
    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;
        GameObject obj = Managers.Resources.Instantiate($"UI/SubItem/{name}");

        if(parent != null)
            obj.transform.SetParent(parent);
        
        return obj.GetOrAddComponent<T>();
    }

    public T ShowSceneUI<T>(string name = null)where T: UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject obj = Managers.Resources.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(obj);
        _sceneUI = sceneUI;

        obj.transform.SetParent(Root.transform);
        
        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null) where T: UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject obj = Managers.Resources.Instantiate($"UI/Popup/{name}");
        T popupUI = Util.GetOrAddComponent<T>(obj);
        _popupStack.Push(popupUI);

        obj.transform.SetParent(Root.transform);
        
        return popupUI;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (popup == null)
            return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("@ERROR@ 맨 위의 팝업이 아닌 다른 팝업을 지우려고 시도했습니다. ");
            return;
        }
        
        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;
        UI_Popup popup = _popupStack.Pop();
        Managers.Resources.Destroy(popup.gameObject);
        popup = null;

        _order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }
}
