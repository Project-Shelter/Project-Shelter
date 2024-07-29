using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAttackReload : ActorAttackState
{
    private IRangeWeapon rangeWeapon;
    private float reloadDelay;
    private float reloadDuration;
    public ActorAttackReload(Actor actor) : base(actor) { }

    public override void EnterState()
    {
        rangeWeapon = Actor.Weapon as IRangeWeapon;
        reloadDelay = rangeWeapon.ReloadDelay;
        reloadDuration = 0f;
        Actor.Anim.SetAnimParamter(ActorAnimParameter.IsAttacking, true);
        Actor.ReloadSlider.maxValue = reloadDelay;
        Actor.ReloadSlider.gameObject.SetActive(true);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        reloadDuration += Time.deltaTime;
        Actor.ReloadSlider.value = reloadDuration;
    }

    public override void FixedUpdateState()
    {
    }

    public override void ExitState()
    {
        Actor.ReloadSlider.gameObject.SetActive(false);
        if (reloadDuration >= reloadDelay)
        {
            rangeWeapon.Reload();
        }
        reloadDuration = reloadDelay;
    }

    protected override void ChangeFromState()
    {
        if(!Actor.StateMachine.CanAttackStates.Contains(Actor.StateMachine.CurrentState))
        {
            Actor.AttackStateMachine.SetState(AttackState.Idle);
            return;
        }
        if (reloadDuration >= reloadDelay || Actor.IsDead)
        {
            Actor.AttackStateMachine.SetState(AttackState.Idle);
            return;
        }
    }
}
