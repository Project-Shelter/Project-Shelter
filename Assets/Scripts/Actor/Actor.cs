using System;
using UnityEngine;

//Actor의 정보(Stat, MovementState) 및 정보 업데이트
public partial class Actor : MonoBehaviour, ILivingEntity
{
    #region ActorStates

    [SerializeField] private bool isHumanActor = false;
    public bool CanSwitch { get { return InputHandler.ButtonCtrl; } }
    public bool IsDead { get { return health.IsDead; } }
    public float HP { get { return health.HP; } }

    #endregion
    #region Variables

    public Transform Tr { get; private set; }
    public Collider2D Coll { get; private set; }
    public ActorStat Stat { get; private set; } // = new ActorStat(); //추후 부활 (인스펙터에서 수치변동용)
    public ActorStateMachine StateMachine { get; private set; }
    public ActorAnimController Anim { get; private set; }
    public ActorMoveBody MoveBody { get; private set; }
    public BuffAttacher Buff { get; private set; }
    private ActorHealth health;
    public Satiety Satiety { get; private set; }
    public ParticleSystem ActorSwitchEffect { get; private set; }

    #endregion

    private void Awake()
    {
        InitVariables();
        LateInitVariables();
        InitHelath();
    }

    public void ActorUpdate()
    {
        StateMachine.StateUpdate();
    }

    public void ActorFixedUpdate()
    {
        StateMachine.StateFixedUpdate();
    }

    public void EnterControl() 
    {
        Camera.main.cullingMask |= 1 << gameObject.layer;
        if(roof != null) roof.SetActive(false);
    }

    public void ExitControl()
    {
        if (!IsDead) StateMachine.SetState(ActorState.Idle);
        else StateMachine.SetState(ActorState.Die);

        if(gameObject.layer != (int)Define.Layer.Ground) Camera.main.cullingMask &= ~(1 << gameObject.layer);
        if(roof != null) roof.SetActive(true);
    }

    private void InitVariables()
    {
        Tr = transform;
        Coll = Util.GetOrAddComponent<Collider2D>(gameObject);
        Stat = GetComponent<ActorStat>(); //추후 삭제 (인스펙터에서 수치변동용)
        StateMachine = new ActorStateMachine(this);
        MoveBody = GetComponent<ActorMoveBody>();
        Anim = new ActorAnimController(this);
        Buff = new BuffAttacher(this);
        ActorSwitchEffect = Util.FindChild<ParticleSystem>(this.gameObject, "ActorSwitchEffect");
    }

    private void LateInitVariables()
    {
        InitCollisionVariables();
        health = new ActorHealth(this, Stat.minHP.GetValue(), Stat.maxHP.GetValue());
        Satiety = new Satiety(this, Stat.minSatiety.GetValue(), Stat.maxSatiety.GetValue(), Stat.satietyIndex.GetValue());
    }
}

