using Unity.VisualScripting.FullSerializer;
using UnityEngine;
public class ActorAttackIdle : ActorAttackState
{
    public ActorAttackIdle(Actor actor) : base(actor){}

    public override void EnterState()
    {
        Actor.Anim.SetAnimParamter(ActorAnimParameter.IsAttacking, true);
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void FixedUpdateState()
    {
    }

    public override void ExitState()
    {
    }

    protected override void ChangeFromState()
    {
        if(Actor.Weapon == null)
        {
            Actor.AttackStateMachine.SetState(AttackState.Disarm);
            return;
        }

        if (Actor.CanAttack)
        {
            if (Actor.Weapon is IRangeWeapon range && range.CurrentAmmo > 0)
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
            if (Actor.Weapon is IRangeWeapon range && range.MaxAmmo > range.CurrentAmmo)
            {
                Actor.AttackStateMachine.SetState(AttackState.Reload);
                return;
            }
        }
    }
}
