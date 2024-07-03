using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAttackReload : ActorAttackState
{
    private IRangeWeapon rangeWeapon;
    private float reloadDuration;
    public ActorAttackReload(Actor actor) : base(actor) { }

    public override void EnterState()
    {
        rangeWeapon = Actor.Weapon as IRangeWeapon;
        rangeWeapon.Reload();
        reloadDuration = rangeWeapon.ReloadDelay;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        reloadDuration -= Time.deltaTime;
    }

    public override void FixedUpdateState()
    {
    }

    public override void ExitState()
    {
        Actor.Anim.SetAnimParamter(ActorAnimParameter.IsAttacking, false);
    }

    protected override void ChangeFromState()
    {
        if (reloadDuration < 0f || Actor.IsDead)
        {
            Actor.AttackStateMachine.SetState(AttackState.Idle);
        }
    }
}
