using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class Monster : MonoBehaviour, ILivingEntity, IMovable
{

    #region Variables

    public Transform Tr { get; private set; }
    public Collider2D Coll { get; private set; }
    public Animator Anim { get; private set; }
    public SpriteRenderer Sprite { get; private set; }
    public MonsterStat Stat { get; private set; }
    private MonsterHealth health;
    public bool IsDead { get { return health.IsDead; } }
    public MonsterMoveBody MoveBody { get; private set; }
    public MonsterAttacker Attacker { get; private set; }
    public MonsterStateMachine StateMachine { get; private set; }
    public ILivingEntity DetectedTarget { get 
        {
            if (detectedTarget == null || detectedTarget.IsDead) return null;
            if (detectedTarget is Actor actor && actor.StateMachine.CurrentState == ActorState.Interact) return actor.Concealment;
            return detectedTarget;
        } 
    }
    private ILivingEntity detectedTarget;
    public ILivingEntity AttackTarget { get; set; }
    public BreakableObject ObstacleTarget { get; set; }

    #endregion

    private void InitVariables()
    {
        Tr = transform;
        Coll = Util.GetOrAddComponent<Collider2D>(gameObject);
        Anim = Util.GetOrAddComponent<Animator>(gameObject);
        Sprite = Util.GetOrAddComponent<SpriteRenderer>(gameObject);
        Stat = GetComponent<MonsterStat>();
        health = new MonsterHealth(this, Stat.minHP, Stat.maxHP);
        MoveBody = new MonsterMoveBody(this);
        Attacker = new MonsterAttacker(this);
        StateMachine = new MonsterStateMachine(this);
        detectedTarget = null;
        AttackTarget = null;
    }

    private void Start()
    {
        InitVariables();
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
        print(target + " detected");
        detectedTarget = target;
    }

    public Vector2 GetVelocity()
    {
        return MoveBody.Velocity;
    }
}
