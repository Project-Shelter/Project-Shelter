using ItemContainer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActorUseItem : ActorBaseState
{
    private ItemVO item;
    private ItemEffect itemEffect;

    private float time;
    private float delayTime;
    private float afterDelayTime;
    private Slider useTimeSlider;

    private bool isUsingItem;

    public ActorUseItem(Actor actor) : base(actor) 
    {
        useTimeSlider = Util.FindChild<Slider>(actor.gameObject, "UseTimeSlider", true);
        useTimeSlider.gameObject.SetActive(false);
    }

    public override void EnterState()
    {
        Actor.MoveBody.Stop();

        item = Actor.Item;
        int effectID = ItemDummyData.ItemEffectRelations[item.id][0];
        itemEffect = ItemDummyData.ItemEffects[effectID];

        delayTime = itemEffect.Runtime;
        afterDelayTime = itemEffect.AfterRuntime;

        useTimeSlider.maxValue = delayTime;
        useTimeSlider.value = 0;
        useTimeSlider.gameObject.SetActive(true);

        time = 0;
        isUsingItem = true;
    }

    public override void UpdateState()
    {
        time += Time.deltaTime;
        if (time <= delayTime)
        {
            useTimeSlider.value = time;
        }
        else
        { 
            UseItem();
        }
        base.UpdateState();
    }

    public override void FixedUpdateState() { }

    public override void UpdateWithNoCtrl() { }

    public override void ExitState() 
    {
        useTimeSlider.gameObject.SetActive(false);
    }

    protected override void ChangeFromState()
    {
        if (Actor.IsDead)
        {
            Actor.StateMachine.SetState(ActorState.Die);
            return;
        }
        if (item == null || !CanKeepUsingItem())
        {
            Actor.StateMachine.SetState(ActorState.Idle);
            return;
        }
    }

    private void UseItem()
    {
        isUsingItem = false;
        Actor.Item.Count--;
        if(itemEffect.Type == EffectType.Heal)
        {
            Actor.RestoreHP(itemEffect.Value);
        }
        else if(itemEffect.Type == EffectType.HealHunger)
        {
            Actor.Satiety.RestoreSatiety(itemEffect.Value); 
        }
    }

    private bool CanKeepUsingItem()
    {
        bool isNotChanged = Actor.Item != null && Actor.Item == item;
        bool isNotZero = Actor.Item.Count > 0;
        bool nonInput = !InputHandler.ButtonAny;
        bool clickLeft = InputHandler.ClickLeft;
        return isNotChanged && isNotZero && nonInput && isUsingItem && clickLeft;
    }
}
