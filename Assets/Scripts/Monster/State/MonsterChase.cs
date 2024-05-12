using UnityEngine;

public class MonsterChase : MonsterBaseState
{
    public MonsterChase(in MonsterStateManager manager) : base(manager) { }

    private GameObject target;

    static bool isStayingAggro;
    private float timeForMinusAggro;
    private const float TIME_BET_MINUS_AGGRO = 1.0f;

    public int chasePoint;
    
    public override void OnStateEnter()
    {
        Manager.Animator.SetFloat("MovingBlend", 1.0f);

        target = Manager.ChasingTarget.gameObject;

        isStayingAggro = true;
        chasePoint = Manager.Stat.chasePoint;
    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateFixedUpdate()
    {
        Manager.canAttack = CanAttack();
        CheckAggro();
        Chase();
    }

    private void CheckAggro()
    {
        bool canKeepChasing;
        if (IsTargetInRadius() && IsTargetInHeight())
        {
            canKeepChasing = true;
        }
        else
        {
            canKeepChasing = false;
        }

        if (canKeepChasing)
        {
            isStayingAggro = true;
            chasePoint = Manager.Stat.chasePoint;
        }
        else
        {
            if (isStayingAggro)
            {
                isStayingAggro = false;
                timeForMinusAggro = Time.time;
            }
            if (timeForMinusAggro + TIME_BET_MINUS_AGGRO <= Time.time)
            {
                chasePoint--;
                timeForMinusAggro = Time.time;
            }
        }

        if (chasePoint == 0)
        {
            Manager.ChasingTarget = null;
        }
    }

    private bool IsTargetInRadius()
    {
        float distFromTarget = Vector2.Distance(Manager.Rigid.position, target.transform.position);
        return distFromTarget <= Manager.Stat.chaseRadius.GetValue();
    }

    private bool IsTargetInHeight()
    {
        float yDistFromTarget = target.transform.position.y - Manager.Rigid.position.y;
        return yDistFromTarget <= Manager.Stat.chaseHeight.GetValue();
    }

    private void Chase()
    {
        if (Manager.canAttack)
        {
            return;
        }

        float xDistFromTarget = target.transform.position.x - Manager.Rigid.position.x;
        float direction = Mathf.Sign(xDistFromTarget);
        if (Mathf.Abs(xDistFromTarget) < Manager.Stat.attackRange.GetValue() && Manager.Direction == direction)
        {
            Manager.Animator.SetFloat("MovingBlend", 0.0f);
            direction = 0;
        }
        else
        {
            Manager.Animator.SetFloat("MovingBlend", 1.0f);
        }

        Manager.Move(new Vector2(direction, 0f) * Manager.Stat.ChaseSpeed);
    }

    private bool CanAttack()
    {
        Vector2 collCenter = Manager.Coll.bounds.center;
        Vector2 attackCenter = new(collCenter.x + Manager.Direction * Manager.Stat.attackRange.GetValue() / 2, collCenter.y);
        Vector2 attackSize = new(Manager.Stat.attackRange.GetValue(), Manager.Coll.bounds.size.y);

        Collider2D[] hit = Physics2D.OverlapBoxAll(attackCenter, attackSize, 0, 1 << (int)Define.Layer.Character);
        foreach (Collider2D collider in hit)
        {
            Actor target = collider.GetComponent<Actor>();
            if (target != null && target == Manager.ChasingTarget) { return true; }
        }

        return false;
    }

    public override void OnStateExit()
    {

    }
}
