using UnityEngine;
public class ActorAttackIdle : ActorAttackState
{
    public ActorAttackIdle(Actor actor) : base(actor){}

    public override void EnterState()
    {
        if(Actor.Weapon != null)
        {
            Actor.Anim.SetAnimParamter(ActorAnimParameter.IsAttacking, true);
        }
        else
        {
            Actor.Anim.SetAnimParamter(ActorAnimParameter.IsAttacking, false);
        }
    }

    public override void UpdateState()
    {
        base.UpdateState(); 
        if (Actor.WeaponSocket != null && Actor.Weapon != null)
        {
            Actor.Anim.SetAnimParamter(ActorAnimParameter.IsAttacking, true);
        }
        else
        {
            Actor.Anim.SetAnimParamter(ActorAnimParameter.IsAttacking, false);
        }
    }

    public override void FixedUpdateState()
    {
    }

    public override void ExitState()
    {
    }

    protected override void ChangeFromState()
    {
        if (Actor.CanAttack)
        {
            if (Actor.Weapon is IRangeWeapon range && !range.HasToBeReload)
            {
                Actor.AttackStateMachine.SetState(AttackState.Range);
                return;
            }

            if (Actor.Weapon is IMeleeWeapon)
            {
                Actor.AttackStateMachine.SetState(AttackState.Melee);
                return;
            }
        }

        if (Actor.CanReload)
        {
            if (Actor.Weapon is IRangeWeapon range && range.CanReload)
            {
                Actor.AttackStateMachine.SetState(AttackState.Reload);
                return;
            }
        }
    }
}
