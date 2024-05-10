using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour, IMovable
{

    #region Variables

    public Transform Tr { get; private set; }
    public Collider2D Coll { get; private set; }
    public Vector3 Pos => new (Coll.bounds.center.x, Coll.bounds.center.y - Coll.bounds.extents.y, Tr.position.z);
    public Animator Anim { get; private set; }
    public MonsterStat Stat { get; private set; }
    public MonsterMoveBody MoveBody { get; private set; }
    public MonsterAttacker Attacker { get; private set; }
    public MonsterStateMachine StateMachine { get; private set; }
    public ILivingEntity ChaseTarget { get; set; }
    public ILivingEntity AttackTarget { get; set; }
    public BreakableObject ObstacleTarget { get; set; }

    #endregion

    private void Awake()
    {
        InitVariables();
    }

    private void InitVariables()
    {
        Tr = transform;
        Coll = Util.GetOrAddComponent<Collider2D>(gameObject);
        Anim = Util.GetOrAddComponent<Animator>(gameObject);
        Stat = GetComponent<MonsterStat>();
        MoveBody = new MonsterMoveBody(this);
        Attacker = new MonsterAttacker(this);
        StateMachine = new MonsterStateMachine(this);
        ChaseTarget = null;
        AttackTarget = null;
    }

    private void Start()
    {
        StateMachine.Init("Move");
    }

    private void Update()
    {
        StateMachine.StateUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.StateFixedUpdate();
    }

    public void ChangeFloor(Define.Layer floor)
    {
        gameObject.layer = (int)floor;
    }

    public void DetectTarget(ILivingEntity target)
    {
        ChaseTarget = target;
    }

    public Vector2 GetVelocity()
    {
        return MoveBody.Velocity;
    }
}
