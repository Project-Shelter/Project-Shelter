using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Clock : UI_Section
{
    enum Texts
    {
        TimerText,
    }
    
    private TextMeshProUGUI Timer;
    void Start()
    {
        Init();
    }
    
    private void FixedUpdate()
    {
        SetCurrentTime();
    }

    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        
        //Timer 설정
        Timer = GetText((int)Texts.TimerText);
    }
    
    private void SetCurrentTime()
    {
        Timer.text = Managers.GetCurrentScene<GameScene>().DayNight.GetTimerText();
    }
}
