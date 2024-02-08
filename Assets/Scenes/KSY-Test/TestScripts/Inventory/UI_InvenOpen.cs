using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InvenOpen : MonoBehaviour
{
    [SerializeField]
    public GameObject UI_Inventory;
    public GameObject UI_ChestInventory;
    public GameObject UI_Chest;

    public void Start()
    {
        Inventory.Instance.OpenChest -= OpenChest;
        Inventory.Instance.OpenChest += OpenChest;
    }

    public void OpenInven()
    {
        UI_Inventory.SetActive(true);
    }

    public void OpenChest(int chestNum)
    {
        UI_ChestInventory.SetActive(true);
        UI_Chest.SetActive(true);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(!UI_Inventory.activeSelf && !UI_Chest.activeSelf)
                OpenInven();
        }
    }
}
