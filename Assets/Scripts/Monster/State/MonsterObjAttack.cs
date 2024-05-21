using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterObjAttack : MonsterBaseState
{
    Coroutine attackCoroutine;
    bool canAttack;

    public MonsterObjAttack(MonsterStateMachine stateMachine) : base(stateMachine) {
    }
    public override void EnterState()
    {
        //StateMachine.Owner.Anim.SetBool("Attack", true);
        StateMachine.Owner.MoveBody.Stop();
        attackCoroutine = StateMachine.Owner.StartCoroutine(Attack());
        NavMeshController.Instance.ChangeAgentType(StateMachine.Owner.MoveBody.Agent, Agent.WithoutObjects);
        canAttack = true;
    }

    private IEnumerator Attack()
    {
        yield return null;

        WaitForSeconds attackDelay = new(StateMachine.Owner.Stat.attackDelay.GetValue());
        while (canAttack)
        {
            bool attackSucceeded = StateMachine.Owner.Attacker.Attack(StateMachine.Owner.ObstacleTarget);
            yield return attackDelay;
            canAttack = CanFindTarget();
        }
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

    public override void ExitState()
    {
        //StateMachine.Owner.Anim.SetBool("Attack", false);
        StateMachine.Owner.ObstacleTarget = null;
        StateMachine.Owner.StopCoroutine(attackCoroutine);
    }

    public override void UpdateState()
    {
        base.UpdateState();
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
    }
}
