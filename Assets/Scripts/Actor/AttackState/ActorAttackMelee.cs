using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAttackMelee : ActorAttackState
{
    private IMeleeWeapon meleeWeapon;
    private float attackDuration;
    public ActorAttackMelee(Actor actor) : base(actor) { }

    public override void EnterState()
    {
        meleeWeapon = Actor.Weapon as IMeleeWeapon;
        Actor.Anim.SetAnimParamter(ActorAnimParameter.IsAttacking, true);
        Actor.MoveBody.Turn();
        meleeWeapon.Attack();
        attackDuration = meleeWeapon.AttackDelay;
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
        meleeWeapon.EndAttack();
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
