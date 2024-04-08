using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{

    #region Variables

    public Transform Tr { get; private set; }
    public Collider2D Coll { get; private set; }
    public MonsterStat Stat { get; private set; }
    public MonsterMoveBody MoveBody { get; private set; }
    public MonsterStateMachine StateMachine { get; private set; }

    #endregion

    private void Awake()
    {
        InitVariables();
    }

    private void InitVariables()
    {
        Tr = transform;
        Coll = Util.GetOrAddComponent<Collider2D>(gameObject);
        Stat = GetComponent<MonsterStat>();
        MoveBody = new MonsterMoveBody(this);
        StateMachine = new MonsterStateMachine(this);
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

}
