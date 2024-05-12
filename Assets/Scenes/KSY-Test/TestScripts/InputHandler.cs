using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

//Input 방식 변경... Delegate 혹은 새로운 입력 시스템 사용
public class InputHandler : MonoSingleton<InputHandler>
{
    #region KeyInput

    #region Player

    public static bool ButtonW { get; private set; } = false;
    public static bool ButtonS { get; private set; } = false;
    public static bool ButtonA { get; private set; } = false;
    public static bool ButtonD { get; private set; } = false;
    public static bool ButtonCtrl { get; private set; } = false;
    public static bool ButtonSpace { get; private set; } = false;
    public static bool ButtonEnter { get; private set; } = false;
    public static bool ButtonESC { get; private set; } = false;
    public static bool ButtonR { get; private set; } = false;

    #endregion
    public static bool[] ButtonArray { get; private set; } = new bool[10];
    
    #endregion

    #region MouseInput

    public static bool ClickRight { get; private set; } = false;
    public static bool ClickLeft { get; private set; } = false;

    #endregion

    void Update()
    {
        if (Input.GetKey(KeyCode.W))             ButtonW = true;       else ButtonW = false;
        if (Input.GetKey(KeyCode.S))             ButtonS = true;       else ButtonS = false;
        if (Input.GetKey(KeyCode.A))             ButtonA = true;       else ButtonA = false;
        if (Input.GetKey(KeyCode.D))             ButtonD = true;       else ButtonD = false;
        if (Input.GetKeyDown(KeyCode.LeftControl))             ButtonCtrl = true;    else ButtonCtrl = false;
        if (Input.GetKeyDown(KeyCode.R)) ButtonR = true; else ButtonR = false;
        if (Input.GetKey(KeyCode.Space))         ButtonSpace = true;   else ButtonSpace = false;
        if (Input.GetKey(KeyCode.KeypadEnter))   ButtonEnter = true;   else ButtonEnter = false;
        if (Input.GetKey(KeyCode.Escape))        ButtonESC = true;     else ButtonESC = false;
        if (Input.GetMouseButtonDown(0))             ClickLeft = true;     else ClickLeft = false;
        if (Input.GetMouseButtonDown(1))             ClickRight = true;    else ClickRight = false;

        if (Input.GetKey(KeyCode.Alpha1))        ButtonArray[1] = true;        else ButtonArray[1] = false;
        if (Input.GetKey(KeyCode.Alpha2))        ButtonArray[2] = true;        else ButtonArray[2] = false;
        if (Input.GetKey(KeyCode.Alpha3))        ButtonArray[3] = true;        else ButtonArray[3] = false;
        if (Input.GetKey(KeyCode.Alpha4))        ButtonArray[4] = true;        else ButtonArray[4] = false;
        if (Input.GetKey(KeyCode.Alpha5))        ButtonArray[5] = true;        else ButtonArray[5] = false;
        if (Input.GetKey(KeyCode.Alpha6))        ButtonArray[6] = true;        else ButtonArray[6] = false;
    }

}
