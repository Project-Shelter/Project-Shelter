using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;
using UnityEngine.EventSystems;

public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();
    
    //요소를 enum으로 받아 Hierachy에 있는 오브젝트(Ex. Button, Text)를 _objects에 바인딩한다.
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);

        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];

        if (_objects.ContainsKey(typeof(T)))
        {
            objects.Concat(_objects[typeof(T)]);
            _objects[typeof(T)] = objects;
        }
        else _objects.Add(typeof(T), objects);

        for(int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else 
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);
            
            if(objects[i] == null)
                Debug.Log($"Failed to Bind!{names[i]}");
        }
    }

    //리스트로 요소의 이름을 받아 Hierachy에 있는 오브젝트를 _objects에 바인딩한다.
    protected void Bind<T>(string[] list) where T : UnityEngine.Object{
        UnityEngine.Object[] objects = new UnityEngine.Object[list.Length];
        if (_objects.ContainsKey(typeof(T)))
        {
            objects.Concat(_objects[typeof(T)]);
            _objects[typeof(T)] = objects;
        }
        else _objects.Add(typeof(T), objects);

        for(int i = 0; i < list.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, list[i], true);
            else 
                objects[i] = Util.FindChild<T>(gameObject, list[i], true);
            
            if(objects[i] == null)
                Debug.Log($"Failed to Bind!{list[i]}");
        }
    }
    
    //오브젝트의 컴포넌트를 가져온다. Get보다는 GetText, GetButton과 같이 아래 만들어둔 함수의 사용을 권장한다.
    protected T Get<T>(int index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[index] as T;
    }

    protected TextMeshProUGUI GetText(int index)
    {
        return Get<TextMeshProUGUI>(index);
    }
    
    protected Button GetButton(int index)
    {
        return Get<Button>(index);
    }
    
    protected Image GetImage(int index)
    {
        return Get<Image>(index);
    }

    protected GameObject GetObject(int index)
    {
        return Get<GameObject>(index);
    }
    
    //오브젝트에 UI 이벤트를 바인딩한다. Click, Drag 외 이벤트가 필요하다면 UI_EventHandler 에 추가 후 해당 함수를 변경
    public static void BindUIEvent(GameObject obj, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(obj);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
    }
}
