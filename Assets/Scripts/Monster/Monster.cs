using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour, IMovable
{

    #region Variables

    public Transform Tr { get; private set; }
    public Collider2D Coll { get; private set; }
    public Animator Anim { get; private set; }
    public MonsterStat Stat { get; private set; }
    public MonsterMoveBody MoveBody { get; private set; }
    public MonsterStateMachine StateMachine { get; private set; }
    public Transform Target { get; set; }

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
        StateMachine = new MonsterStateMachine(this);
        Target = null;
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

    public void DetectTarget(Transform target)
    {
        Target = target;
    }

    public Vector2 GetVelocity()
    {
        return MoveBody.Velocity;
    }
}
