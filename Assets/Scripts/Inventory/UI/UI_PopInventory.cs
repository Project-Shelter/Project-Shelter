using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PopInventory : UI_PopUI
{
    public override void Update()
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
}
