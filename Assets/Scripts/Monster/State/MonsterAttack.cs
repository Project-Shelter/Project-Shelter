using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonsterBaseState
{
    bool canAttack;
    bool isAttacking;

    public MonsterAttack(MonsterStateMachine stateMachine) : base(stateMachine) 
    {
        StateMachine.Owner.Attacker.OnAttackEnded += () => StateMachine.Owner.StartCoroutine(DelayAttack());
    }

    public override void EnterState()
    {
        StateMachine.Owner.Anim.SetBool("Attack", true);
        StateMachine.Owner.MoveBody.Stop();
        canAttack = CanFindTarget();
        isAttacking = true;

    }
    public override void UpdateState()
    {
        if (!isAttacking) 
        { 
            canAttack = CanFindTarget() && IsTargetInAttackRange();
            if(canAttack)
            {
                isAttacking = true;
                StateMachine.Owner.Anim.Play("Attack", -1, 0f);
            }
        }
        base.UpdateState();

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

    private bool IsTargetInAttackRange()
    {
        Direction dir = StateMachine.Owner.MoveBody.MoveDir;
        Collider2D[] colls = StateMachine.Owner.Attacker.GetCollsInAttackRange(dir);
        foreach (Collider2D coll in colls)
        {
            if (coll == StateMachine.Owner.AttackTarget.Coll)
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator DelayAttack()
    {
        WaitForSeconds attackDelay = new(StateMachine.Owner.Stat.attackDelay.GetValue());
        yield return attackDelay;
        isAttacking = false;
    }

    public override void ExitState()
    {
        StateMachine.Owner.Anim.SetBool("Attack", false);
        StateMachine.Owner.AttackTarget = null;
    }

    public override void FixedUpdateState() { }

    protected override void ChangeFromState()
    {
        if (StateMachine.Owner.IsDead)
        {
            StateMachine.SetState("Die");
            return;
        }

        if (!canAttack)
        {
            if(StateMachine.Owner.DetectedTarget != null)
            {
                StateMachine.SetState("Chase");
                return;
            }
            else
            {
                StateMachine.SetState("Move");
                return;
            }
        }
    }
}