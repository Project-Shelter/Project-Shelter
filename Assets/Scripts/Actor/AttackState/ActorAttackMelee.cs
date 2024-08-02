using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAttackMelee : ActorAttackState
{
    private IMeleeWeapon meleeWeapon;
    private float attackDuration;
    private float afterAttackDuration;
    public ActorAttackMelee(Actor actor) : base(actor) { }

    public override void EnterState()
    {
        meleeWeapon = Actor.Weapon as IMeleeWeapon;
        Actor.Anim.SetAnimParamter(ActorAnimParameter.IsAttacking, true);
        Actor.MoveBody.Turn();
        meleeWeapon.Attack();
        attackDuration = meleeWeapon.AttackDelay;
        afterAttackDuration = meleeWeapon.AfterAttackDelay;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        attackDuration -= Time.deltaTime;
        if (attackDuration < 0)
        {
            if(afterAttackDuration == meleeWeapon.AfterAttackDelay)
            {
                meleeWeapon.AfterAttack();
            }
            afterAttackDuration -= Time.deltaTime;
        }
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
        if (afterAttackDuration < 0f || Actor.IsDead)
        {
            Actor.AttackStateMachine.SetState(AttackState.Idle);
        }
    }
}
