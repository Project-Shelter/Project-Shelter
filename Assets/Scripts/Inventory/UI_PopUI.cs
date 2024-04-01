using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PopUI : MonoBehaviour
{
    public GameObject UIobj;
    private Button popupBtn;

    public void Awake()
    {
        popupBtn = gameObject.GetComponent<Button>();
    }
    public void Start(){
        popupBtn.onClick.AddListener(Open);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(UIobj.activeSelf) Close();
            else Open();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(UIobj.activeSelf) Close();
        }
    }

    public void Open(){
        UIobj.SetActive(true);
    }

    public void Close(){
        UIobj.SetActive(false);
    }
}
