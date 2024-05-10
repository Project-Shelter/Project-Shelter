using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterObjAttack : MonsterBaseState
{
    Coroutine attackCoroutine;

    public MonsterObjAttack(MonsterStateMachine stateMachine) : base(stateMachine) {
    }
    public override void EnterState()
    {
        //StateMachine.Owner.Anim.SetBool("Attack", true);
        StateMachine.Owner.MoveBody.Stop();
        attackCoroutine = StateMachine.Owner.StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        yield return null;

        WaitForSeconds attackDelay = new(StateMachine.Owner.Stat.attackDelay.GetValue());
        while (CanAttack())
        {
            StateMachine.Owner.Attacker.Attack(StateMachine.Owner.ObstacleTarget);
            yield return attackDelay;
        }
    }

    private bool CanAttack()
    {
        if (StateMachine.Owner.ObstacleTarget == null || StateMachine.Owner.ObstacleTarget.IsDead)
        {
            return false;
        }
        if(StateMachine.Owner.ChaseTarget == null)
        {
            return false;
        }
        
        Vector2 targetPos = StateMachine.Owner.ChaseTarget.Coll.transform.position;
        NavMeshPath path = new NavMeshPath();
        StateMachine.Owner.MoveBody.Agent.CalculatePath(targetPos, path);
        BreakableObject obj = StateMachine.Owner.Attacker.FindObstacleObj(path);
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
        if (!CanAttack())
        {
            if (StateMachine.Owner.ChaseTarget != null)
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
