using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonsterBaseState
{
    Coroutine attackCoroutine;
    bool canAttack;

    public MonsterAttack(MonsterStateMachine stateMachine) : base(stateMachine) {}

    public override void EnterState()
    {
        //StateMachine.Owner.Anim.SetBool("Attack", true);
        StateMachine.Owner.MoveBody.Stop();
        canAttack = true;
        attackCoroutine = StateMachine.Owner.StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        yield return null;

        WaitForSeconds attackDelay = new(StateMachine.Owner.Stat.attackDelay.GetValue());
        while (canAttack)
        {
            bool isAttackSucceeded = StateMachine.Owner.Attacker.Attack(StateMachine.Owner.AttackTarget);
            yield return attackDelay;
            canAttack = CanFindTarget() && isAttackSucceeded;
        }
    }

    private bool CanFindTarget()
    {
        if (StateMachine.Owner.AttackTarget == null || StateMachine.Owner.AttackTarget.IsDead)
        {
            return false;
        }
        if(StateMachine.Owner.AttackTarget != StateMachine.Owner.DetectedTarget)
        {
            return false;
        }
        return true;
    }

    public override void ExitState()
    {
        //StateMachine.Owner.Anim.SetBool("Attack", false);
        StateMachine.Owner.AttackTarget = null;
        StateMachine.Owner.StopCoroutine(attackCoroutine);
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void FixedUpdateState()
    {
    }

    protected override void ChangeFromState()
    {
        if(!canAttack)
        {
            if(StateMachine.Owner.DetectedTarget != null)
            {
                StateMachine.SetState("Chase");
            }
            else
            {
                StateMachine.SetState("Move");
            }
        }
    }
}