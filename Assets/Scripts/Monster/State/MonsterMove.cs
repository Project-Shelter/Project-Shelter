using UnityEngine;
using UnityEngine.Diagnostics;
using static MonsterStat;

public class MonsterMove : MonsterBaseState
{
    #region Move Variables

    private MoveType moveType;
    private float moveDirection;
    private bool isInitialState = true;
    private bool isMoving;

    private Transform[] PatrolPoints { get => Manager.Stat.patrolPoints; }
    private int patrolPointIdx = -1;

    private float randomDestX;

    #endregion

    #region Observer Variables

    private MonsterObserver observer;
    private Vector2 ObserverPoint { get { return (Vector2)Manager.Coll.bounds.center; } }
    private Vector2 ObserverOffset { get { return new (Manager.Direction * Manager.Stat.ViewDistance.x / 2, 0f); } }

    #endregion

    public MonsterMove(in MonsterStateManager manager) : base(manager)
    {
        observer = new MonsterObserver();
    }

    public override void OnStateEnter()
    {
        if (isInitialState)
        {
            moveType = Manager.Stat.initialMoveType;
        }
        else
        {
            if (moveType == MoveType.Idle)
            {
                moveType = MoveType.Random;
            }
        }

        switch (moveType)
        {
            case MoveType.Idle:
                Manager.Animator.SetFloat("MovingBlend", 0f);
                break;
            default:
                Manager.Animator.SetFloat("MovingBlend", 0.5f);
                break;
        }

        isMoving = false;
    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateFixedUpdate()
    {
        Move();
        Manager.ChasingTarget = observer.Observe(ObserverPoint, ObserverOffset, Manager.Stat.ViewDistance);
    }

    public override void OnStateExit()
    {
        isInitialState = false;
    }

    private void Move()
    {
        switch (moveType)
        {
            case MoveType.Idle:
                Manager.Move(Vector2.zero);
                break;
            case MoveType.Patrol:
                PatrolMove();
                break;
            case MoveType.Random:
                RandomMove();
                break;
        }
    }

    #region Patrol

    private void PatrolMove()
    {
        if (PatrolPoints.Length == 0)
        {
            return;
        }

        if (!isMoving)
        {
            SetNextPatrolPoint();
        }
        else
        {
            isMoving = !IsArrivedAtPoint(PatrolPoints[patrolPointIdx].position.x);
        }

        Manager.Move(new Vector2(moveDirection, 0f) * Manager.Stat.MoveSpeed);
    }

    private void SetNextPatrolPoint()
    {
        patrolPointIdx = (patrolPointIdx + 1) % PatrolPoints.Length;

        float posDiff = PatrolPoints[patrolPointIdx].position.x - Manager.Rigid.position.x;
        if (posDiff > 0)
        {
            moveDirection = 1f;
        }
        else if (posDiff < 0)
        {
            moveDirection = -1f;
        }
        else
        {
            moveDirection = 0f;
        }

        Manager.Move(new Vector2(moveDirection, 0f) * Manager.Stat.MoveSpeed);
        isMoving = true;
    }

    #endregion

    #region Random

    private void RandomMove()
    {
        if (!isMoving)
        {
            SetRandomDestination();
        }
        isMoving = !IsArrivedAtPoint(randomDestX);

        Manager.Move(new Vector2(moveDirection, 0f) * Manager.Stat.MoveSpeed);
    }

    private void SetRandomDestination()
    {
        moveDirection = Mathf.Sign(Random.Range(-1, 1));

        randomDestX = Manager.Rigid.position.x + Random.Range(0f, Manager.Stat.randomMoveDistance.GetValue()) * moveDirection;
        isMoving = true;
    }

    #endregion

    private bool IsArrivedAtPoint(float point)
    {
        float posDiff = point - Manager.Rigid.position.x;
        if (posDiff * moveDirection <= 0)
        {
            moveDirection = 0f;
            return true;
        }

        return false;
    }
}
