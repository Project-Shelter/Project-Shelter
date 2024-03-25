using System.Collections.Generic;
using UnityEngine;

public class MonsterStateManager : MonoBehaviour
{

    #region Const Values

    public readonly Quaternion Left = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    public readonly Quaternion Right = Quaternion.Euler(new Vector3(0f, 180f, 0f));

    #endregion

    #region Variables

    public MonsterStat Stat { get; private set; }
    public Rigidbody2D Rigid { get; private set; }
    public Animator Animator { get; private set; }
    public Collider2D Coll { get; private set; }
    public Actor ChasingTarget { get; set; }

    public bool CanChase
    {
        get
        {
            if (ChasingTarget == null) return false;
            else return !ChasingTarget.IsDead;
        }
    }
    public bool isHit;
    public bool canAttack;
    public bool isInjured;
    public bool IsDead { get { return Stat.IsDead; } }
    public float Direction { get { if (transform.rotation == Left) return -1; else return 1; } }

    public enum AnimationLayer
    {
        Movement = 0,
        Attack = 1,
        Injured = 2,
        Die = 3
    }

    #endregion

    #region FSM

    public enum MonsterState
    {
        Move,
        Chase,
        Attack,
        Injured,
        Die,
    }
    public MonsterState currentState;
    private Dictionary<MonsterState, MonsterBaseState> monsterStates = new();

    public void SetState(MonsterState nextState)
    {
        if (currentState == nextState && currentState != MonsterState.Injured)
        {
            return;
        }

        monsterStates[currentState].OnStateExit();
        currentState = nextState;
        monsterStates[currentState].OnStateEnter();
    }

    #endregion

    // 추후 경사/계단 지형 추가시 이동 매커니즘 재구현
    public void Move(Vector2 moveVelocity)
    {
        if(moveVelocity.x < 0)
        {
            transform.localRotation = Left;
        }
        else if(moveVelocity.x > 0)
        {
            transform.localRotation = Right;
        }

        Rigid.velocity = new(moveVelocity.x, Rigid.velocity.y);
    }

    private void Awake()
    {
        InitVariables();
        InitStates();
    }

    private void InitVariables()
    {
        Stat = GetComponent<MonsterStat>();
        Rigid = GetComponent<Rigidbody2D>();
        Animator = GetComponentInChildren<Animator>();
        Coll = GetComponentInChildren<Collider2D>();

        isHit = false;
        canAttack = false;
        isInjured = false;
    }

    private void InitStates()
    {
        monsterStates.Add(MonsterState.Move, new MonsterMove(this));
        monsterStates.Add(MonsterState.Chase, new MonsterChase(this));
        monsterStates.Add(MonsterState.Attack, new MonsterAttack(this));
        monsterStates.Add(MonsterState.Injured, new MonsterInjured(this));
        monsterStates.Add(MonsterState.Die, new MonsterDie(this));
    }

    private void Start()
    {
        currentState = MonsterState.Move;
        monsterStates[currentState].OnStateEnter();
    }

    private void Update()
    {
        monsterStates[currentState].OnStateUpdate();
        MonsterStateChanger.ChangeState(this);
    }

    private void FixedUpdate()
    {
        monsterStates[currentState].OnStateFixedUpdate();
    }
}
