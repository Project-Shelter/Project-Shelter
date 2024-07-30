using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterObjAttack : MonsterBaseState
{
    bool canAttack;
    bool isAttacking;

    public MonsterObjAttack(MonsterStateMachine stateMachine) : base(stateMachine) 
    {
        StateMachine.Owner.Attacker.OnAttackEnded += () => StateMachine.Owner.StartCoroutine(DelayAttack());
    }

    public override void EnterState()
    {
        StateMachine.Owner.Anim.SetBool("Attack", true);
        StateMachine.Owner.MoveBody.Stop();
        NavMeshController.Instance.ChangeAgentType(StateMachine.Owner.MoveBody.Agent, Agent.WithoutObjects);
        canAttack = true;
        isAttacking = true;
    }

    public override void UpdateState()
    {
        if (!isAttacking)
        {
            canAttack = CanFindTarget() && IsTargetInAttackRange();
            if (canAttack)
            {
                isAttacking = true;
                StateMachine.Owner.Anim.Play("Attack", -1, 0f);
            }
        }
        base.UpdateState();
    }

    private bool CanFindTarget()
    {
        if (StateMachine.Owner.ObstacleTarget == null || StateMachine.Owner.ObstacleTarget.IsDead)
        {
            return false;
        }
        if(StateMachine.Owner.DetectedTarget == null)
        {
            return false;
        }
        
        if(StateMachine.Owner.DetectedTarget == (ILivingEntity)StateMachine.Owner.ObstacleTarget)
        {
            return true;
        }

        Vector2 targetPos = StateMachine.Owner.DetectedTarget.Coll.bounds.center;
        NavMeshPath pathToTarget = new NavMeshPath();
        StateMachine.Owner.MoveBody.Agent.CalculatePath(targetPos, pathToTarget);
        BreakableObject obj = StateMachine.Owner.Attacker.FindObstacleObj(pathToTarget);
        if (obj == null || obj != StateMachine.Owner.ObstacleTarget)
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
            if (coll == StateMachine.Owner.ObstacleTarget.Coll)
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
        StateMachine.Owner.ObstacleTarget = null;
    }

    public override void FixedUpdateState() {}

    protected override void ChangeFromState()
    {
        if (!canAttack)
        {
            if (StateMachine.Owner.DetectedTarget != null)
            {
                StateMachine.SetState("Chase");
            }
            else
            {
                StateMachine.SetState("Move");
            }
        }

        if (StateMachine.Owner.IsDead)
        {
            StateMachine.SetState("Die");
            return;
        }
    }
}
