using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAttackMelee : ActorAttackState
{
    private float attackDuration;
    public ActorAttackMelee(Actor actor) : base(actor) { }

    public override void EnterState()
    {
        Actor.Anim.SetAnimParamter(ActorAnimParameter.IsAttacking, true);
        Actor.MoveBody.Turn();
        Actor.Weapon.Attack();
        attackDuration = Actor.Weapon.AttackDelay;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        attackDuration -= Time.deltaTime;
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
        if (attackDuration < 0f || Actor.IsDead)
        {
            Actor.AttackStateMachine.SetState(AttackState.Idle);
        }
    }
}
