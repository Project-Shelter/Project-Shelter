using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupInventory : PopupContainer
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(go.activeSelf) Close();
            else Open();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(go.activeSelf) Close();
        }
    }
}
