using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Actor : MonoBehaviour, ILivingEntity, IMovable
{
    #region ActorStates

    [SerializeField] private bool isHumanActor = false;
    public bool CanSwitch { get { return InputHandler.ButtonCtrl && StateMachine.CanSwitchStates.Contains(StateMachine.CurrentState); } }
    public bool IsSwitching { get; private set; } 
    public bool CanInteract { get { return InputHandler.ButtonE && interactable != null; } }
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
    public ActorActionRadius ActionRadius { get; private set; }
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

    private void Update()
    {
        StateMachine.StateUpdateWithNoCtrl();
    }
    public void ActorUpdate()
    {
        StateMachine.StateUpdate();
        if(CanSwitch) { StartCoroutine(Switch()); }
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
        IsSwitching = false;
        if (gameObject.layer != (int)Define.Layer.Ground) Camera.main.cullingMask &= ~(1 << gameObject.layer);
        if(roof != null) roof.SetActive(true);
    }

    private IEnumerator Switch()
    {
        if(StateMachine.CurrentState == ActorState.Walk) { StateMachine.SetState(ActorState.Idle); }
        IsSwitching = true;
        ActorSwitchEffect.Play();
        while (ActorSwitchEffect.IsAlive(true)) { yield return null; }

        ActorSwitchEffect.Stop();
        Managers.Scene.GetCurrentScene<GameScene>().ActorController.SwitchActor();
    }

    private void InitVariables()
    {
        Tr = transform;
        Coll = Util.GetOrAddComponent<Collider2D>(gameObject);
        Stat = GetComponent<ActorStat>(); //추후 삭제 (인스펙터에서 수치변동용)
        StateMachine = new ActorStateMachine(this);
        MoveBody = GetComponent<ActorMoveBody>();
        ActionRadius = new ActorActionRadius(this);
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

