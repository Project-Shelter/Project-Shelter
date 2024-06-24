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

    private ActorController actorController;

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
        actorController = Managers.GetCurrentScene<GameScene>().ActorController;
    }

    private void GetPlayerHP()
    {
        hpText.text = hpIndexText + actorController.CurrentActor.HP.ToString() + "%";
    }

    private void GetPlayerSatiety()
    {
        satietyText.text = satietyIndexText + actorController.CurrentActor.Satiety.Value.ToString() + "%";
    }
}
