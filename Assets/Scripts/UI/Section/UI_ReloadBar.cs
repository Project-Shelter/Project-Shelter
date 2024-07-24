using UnityEngine;
using UnityEngine.UI;

public class UI_ReloadBar : UI_Section
{
    enum Bars
    {
        ReloadBar,
    }

    private ActorController controller;
    private Actor Actor => controller.CurrentActor;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Slider>(typeof(Bars));
        Get<Slider>((int)Bars.ReloadBar).gameObject.SetActive(false);

        controller = ServiceLocator.GetService<ActorController>();
        controller.BeforeSwitchActorAction += ResyncActor;
        controller.SwitchActorAction += SetActor;
        SetActor();
    }

    private void SetActor()
    {
        controller.CurrentActor.ReloadAction += SetBarValue;
    }

    private void ResyncActor()
    {
        Get<Slider>((int)Bars.ReloadBar).gameObject.SetActive(false);
        controller.CurrentActor.ReloadAction -= SetBarValue;
    }

    private void SetBarValue(float reloadDelay, float reloadDuration)
    {
        if(reloadDuration >= reloadDelay)
        {
            Get<Slider>((int)Bars.ReloadBar).gameObject.SetActive(false);
            return;
        }
        Get<Slider>((int)Bars.ReloadBar).gameObject.SetActive(true);
        Get<Slider>((int)Bars.ReloadBar).value = reloadDuration / reloadDelay;

        transform.position = Camera.main.WorldToScreenPoint(Actor.Tr.position + Vector3.up * 2.2f);
    }
}
