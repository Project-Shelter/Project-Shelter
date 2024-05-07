using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonsterBaseState
{
    Coroutine attackCoroutine;
    bool isTargetObject;
    bool canAttack;
    Vector2 ClosestTargetPoint => StateMachine.Owner.AttackTarget.ClosestPoint(StateMachine.Owner.transform.position);

    public MonsterAttack(MonsterStateMachine stateMachine) : base(stateMachine)
    {
        attackCoroutine = null;
    }

    public override void EnterState()
    {
        //StateMachine.Owner.Anim.SetBool("Attack", true);
        StateMachine.Owner.MoveBody.Stop();
        canAttack = true;
        isTargetObject = StateMachine.Owner.AttackTarget.TryGetComponent(out BreakableObject _);
        attackCoroutine = StateMachine.Owner.StartCoroutine(Attack());
        Debug.Log(StateMachine.Owner.AttackTarget.name);
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
            if(StateMachine.Owner.ChaseTarget != null)
            {
                StateMachine.SetState("Chase");
            }
            else
            {
                StateMachine.SetState("Move");
            }
        }
    }

    private IEnumerator Attack()
    {
        yield return null;
        
        WaitForSeconds attackDelay = new(StateMachine.Owner.Stat.attackDelay.GetValue());
        while (canAttack)
        {
            Vector2 attackVector = (ClosestTargetPoint - (Vector2)StateMachine.Owner.transform.position).normalized;
            float attackAngle = Vector2.SignedAngle(Vector2.right, attackVector);
            if (attackAngle < 0) { attackAngle += 360; }

            Collider2D[] hits;
            if (attackAngle >= 315 || attackAngle < 45) hits = StateMachine.Owner.MonsterAttacker.CollsInAttackRange(Direction.Right);
            else if (attackAngle >= 45 && attackAngle < 135) hits = StateMachine.Owner.MonsterAttacker.CollsInAttackRange(Direction.Up);
            else if (attackAngle >= 135 && attackAngle < 225) hits = StateMachine.Owner.MonsterAttacker.CollsInAttackRange(Direction.Left);
            else hits = StateMachine.Owner.MonsterAttacker.CollsInAttackRange(Direction.Down);

            ILivingEntity targetEntity = null;
            foreach (Collider2D hit in hits)
            {
                if (hit == StateMachine.Owner.AttackTarget)
                {
                    targetEntity = StateMachine.Owner.AttackTarget.GetComponent<ILivingEntity>();
                    targetEntity.OnDamage(StateMachine.Owner.Stat.attackDamage.GetValue(), ClosestTargetPoint, attackVector);
                    break;
                }
            }
            if (targetEntity == null || targetEntity.IsDead) canAttack = false;
            yield return attackDelay;
        }
    }
    /*
    private bool CanAttack()
    {
        float distance = Vector2.Distance(StateMachine.Owner.transform.position, ClosestTargetPoint);
        bool isTargetInRange = StateMachine.Owner.Stat.attackRange.GetValue() >= distance;

        targetEntity = StateMachine.Owner.AttackTarget.GetComponent<ILivingEntity>();
        if (targetEntity == null || targetEntity.IsDead || !isTargetInRange)
        {
            Debug.Log(distance);
            return false;
        }

        if (isTargetObject)
        {
            BreakableObject targetObject = StateMachine.Owner.MoveBody.FindBreakableObject(StateMachine.Owner.Stat.attackRange.GetValue());
            if(targetObject == null || targetObject.gameObject != StateMachine.Owner.AttackTarget.gameObject)
            {
                Debug.Log(StateMachine.Owner.AttackTarget.name + " " + targetObject.gameObject.name);
                return false;
            }
        }
        return true;
    }*/
}