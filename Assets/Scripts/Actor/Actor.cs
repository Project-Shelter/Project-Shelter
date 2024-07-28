using ItemContainer;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Actor : MonoBehaviour, ILivingEntity, IMovable
{
    #region ActorStates

    [SerializeField] private bool isHumanActor = false;
    public bool CanSwitch { get 
        { 
            return InputHandler.ButtonCtrl && !IsSwitching &&
                StateMachine.CanSwitchStates.Contains(StateMachine.CurrentState) &&
                AttackStateMachine.CanSwitchStates.Contains(AttackStateMachine.CurrentState) && 
                !Managers.UI.IsPopupOn();
                
        } 
    }
    public bool IsSwitching { get; private set; }
    public bool IsAiming { get; private set; }
    public bool CanInteract { get { return InputHandler.ButtonEDown && Interactable != null && !Managers.UI.IsPopupOn(); } }
    public bool CanAttack { get { return InputHandler.ClickLeftDown && StateMachine.CanAttackStates.Contains(StateMachine.CurrentState) && !Managers.UI.IsPopupOn(); } }
    public bool CanUse { get { return InputHandler.ClickLeftDown && Item != null && !Managers.UI.IsPopupOn(); } }
    public bool CanReload { get { return InputHandler.ButtonR && StateMachine.CanAttackStates.Contains(StateMachine.CurrentState); } }
    public bool IsAttacking { get { return AttackStateMachine.CurrentState == AttackState.Range || AttackStateMachine.CurrentState == AttackState.Melee; } }
    public bool IsDead { get { return health.IsDead; } }
    public float HP { get { return health.HP; } }

    #endregion
    #region Variables

    public Transform Tr { get; private set; }
    public Collider2D Coll { get; private set; }
    public ActorController Controller { get; private set; }
    public ActorStat Stat { get; private set; } // = new ActorStat(); //추후 부활 (인스펙터에서 수치변동용)
    public WeaponSocket WeaponSocket { get; private set; }
    public IWeapon Weapon { get { if (WeaponSocket == null) return null; else return WeaponSocket.Weapon; } }
    public Action<float, float> ReloadAction = null;
    public ItemVO Item { get; private set; }
    public ActorStateMachine StateMachine { get; private set; }
    public ActorAttackStateMachine AttackStateMachine { get; private set; }
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
    }

    private void Start()
    {
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
        AttackStateMachine.StateUpdate();
        if(CanSwitch) { StartCoroutine(Switch()); }
    }

    public void ActorFixedUpdate()
    {
        StateMachine.StateFixedUpdate();
        AttackStateMachine.StateFixedUpdate();
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

    public void SetItem(ItemVO item)
    {
        if (item == null) return;

        if(ItemDummyData.ItemDB.data.TryGetValue(item.id, out ItemData itemData))
        {
            if (itemData.itemType == ItemType.EquipItem)
            {
                WeaponSocket.SetWeapon(itemData);
                Item = null;
            }
            else if (itemData.itemType == ItemType.UseItem)
            {
                WeaponSocket.SetWeapon(null);
                Item = item;
            }
        }
    }

    public void Aim()
    {
        if (InputHandler.ClickRight)
        {
            Cursor.SetCursor(InputHandler.AimCursor, InputHandler.AimHotspot, CursorMode.Auto);
            IsAiming = true;
        }

        if (InputHandler.ClickRightUp)
        {
            Cursor.SetCursor(InputHandler.DefaultCursor, InputHandler.DefaultHotspot, CursorMode.Auto);
            IsAiming = false;
        }
    }

    private IEnumerator Switch()
    {
        if(StateMachine.CurrentState == ActorState.Walk) { StateMachine.SetState(ActorState.Idle); }
        IsSwitching = true;
        ActorSwitchEffect.Play();
        while (ActorSwitchEffect.IsAlive(true)) { yield return null; }

        ActorSwitchEffect.Stop();
        Controller.SwitchActor();
    }

    private void InitVariables()
    {
        Tr = transform;
        Coll = Util.GetOrAddComponent<Collider2D>(gameObject);
        Stat = GetComponent<ActorStat>(); //추후 삭제 (인스펙터에서 수치변동용)
        MoveBody = GetComponent<ActorMoveBody>();
        WeaponSocket = Util.FindChild<WeaponSocket>(this.gameObject, "WeaponSocket");
        ActionRadius = new ActorActionRadius(this);
        Buff = new BuffAttacher(this);
        ActorSwitchEffect = Util.FindChild<ParticleSystem>(this.gameObject, "ActorSwitchEffect");
    }

    private void LateInitVariables()
    {
        Controller = ServiceLocator.GetService<ActorController>();
        WeaponSocket.Init();
        Anim = new ActorAnimController(this);
        StateMachine = new ActorStateMachine(this);
        AttackStateMachine = new ActorAttackStateMachine(this);
        InitCollisionVariables();
        health = new ActorHealth(this, Stat.minHP.GetValue(), Stat.maxHP.GetValue());
        Satiety = new Satiety(this, Stat.minSatiety.GetValue(), Stat.maxSatiety.GetValue(), Stat.satietyIndex.GetValue());
    }
}
