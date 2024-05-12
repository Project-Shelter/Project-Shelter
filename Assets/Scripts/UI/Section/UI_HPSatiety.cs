using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// For Demo
public class UI_HPSatiety : UI_Section 
{
    enum Texts
    {
        HPText,
        SatietyText,
    }

    private TextMeshProUGUI hpText;
    private TextMeshProUGUI satietyText;

    private string hpIndexText = "<sprite=0>";
    private string satietyIndexText = "<sprite=20>";
    void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        GetPlayerHP();
        GetPlayerSatiety();
    }

    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));

        //Timer 설정
        hpText = GetText((int)Texts.HPText);
        satietyText = GetText((int)Texts.SatietyText);
    }

    private void GetPlayerHP()
    {
        hpText.text = hpIndexText + ActorController.Instance.CurrentActor.HP.ToString() + "%";
    }

    private void GetPlayerSatiety()
    {
        satietyText.text = satietyIndexText + ActorController.Instance.CurrentActor.Satiety.Value.ToString() + "%";
    }
}
