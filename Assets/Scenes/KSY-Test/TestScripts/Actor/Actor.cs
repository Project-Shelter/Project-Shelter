using UnityEngine;

//Actor의 정보(Stat, MovementState) 및 정보 업데이트
public enum ActorState
{
    Idle,
    Move,
    OnAir,
    OnLadder,
    Down,
    Attack,
    NoControl,
    Die
}
public class Actor : MonoBehaviour
{
    #region Const Values

    public readonly Quaternion Left = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    public readonly Quaternion Right = Quaternion.Euler(new Vector3(0f, 180f, 0f));

    #endregion
    
    #region MovementStates

    public bool GoRight { get; private set; } = true;
    public bool OnGround { get; private set; } = false;
    public bool CanGoUp { get; private set; } = false;
    public bool CanGoDown { get; private set; } = false;
    public bool OnLadder { get; private set; } = false; 
    public bool IsAttacking { get; private set; } = false;
    public bool IsDead { get { return Stat.IsDead; } }
    public int CountCollidingEnemy { get; private set; } = 0;
    
    #endregion

    #region Variables

    public Transform Tr { get; private set; }
    public Rigidbody2D Rigid { get; private set; }
    public PlatformerBody Body { get; private set; }
    public Collider2D Coll { get; private set; }
    public Animator Anim { get; private set; }
    public ActorStat Stat { get; private set; } // = new ActorStat(); //추후 부활 (인스펙터에서 수치변동용)
    public ActorStateMachine StateMachine { get; private set; }
    public ActorAnimController ActorAnim { get; private set; }
    public GameObject CurrentGround { get; private set; }
    public GameObject CurrentLadder { get; private set; }
    #endregion

    private void Awake()
    {
        InitVariables();
    }
    
    public void ActorUpdate()
    {
        UpdateBehavior();
        StateMachine.StateUpdate();
        ActorStateChanger.ChangeState(this);
    }

    public void ActorFixedUpdate()
    {
        StateMachine.StateFixedUpdate();
    }

    public void EnterControl() { ActorStateChanger.InitState(this); }

    public void ExitControl(){ }

    // 현재 Actor가 자신이 아닐시 사망처리
    private void Update()
    {
        if(ActorController.Instance.CurrentActor != this)
        {
            if (IsDead) StateMachine.SetState(ActorState.Die);
        }
    }

    private void InitVariables()
    {
        Tr = transform;
        //Rigid = Util.GetOrAddComponent<Rigidbody2D>(gameObject);
        Body = Util.GetOrAddComponent<PlatformerBody>(gameObject);
        Coll = Util.GetOrAddComponent<Collider2D>(gameObject);
        Anim = Util.GetOrAddComponent<Animator>(gameObject);
        Stat = GetComponent<ActorStat>(); //추후 삭제 (인스펙터에서 수치변동용)
        StateMachine = new ActorStateMachine(this);
        ActorAnim = new ActorAnimController(Anim);
    }

    private void UpdateBehavior()
    {
        if (InputHandler.ButtonD) GoRight = true;
        else if (InputHandler.ButtonA) GoRight = false;
        if (StateMachine.CurrentState == ActorState.Attack)
        {
            if (Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) { IsAttacking = false; }
            else IsAttacking = true;
        }
    }
    
    #region Check Collision
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == (int)Define.Layer.Ladder)
        {
            OnLadder = true;
            CurrentLadder = other.gameObject;
        }
        if (other.gameObject.layer == (int)Define.Layer.CanEnterLadderDown)
        {
            CanGoDown = true;
        }
        if (other.gameObject.layer == (int)Define.Layer.CanEnterLadderUp)
        {
            CanGoUp = true;
        }
        if (other.gameObject.layer == (int)Define.Layer.EnemyTrigger)
        {
            CountCollidingEnemy++;
            Stat.AddBuff(Stat.CollidingEnemyDebuff);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == (int)Define.Layer.Ladder)
        {
            OnLadder = false;
        }
        if (other.gameObject.layer == (int)Define.Layer.CanEnterLadderDown)
        {
            CanGoDown = false;
        }
        if (other.gameObject.layer == (int)Define.Layer.CanEnterLadderUp)
        {
            CanGoUp = false;
        }
        if (other.gameObject.layer == (int)Define.Layer.EnemyTrigger)
        {
            CountCollidingEnemy--;
            if (CountCollidingEnemy == 0) 
            {
                Stat.RemoveBuff(Stat.CollidingEnemyDebuff);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == (int)Define.Layer.Ground)
        {
            OnGround = true;
            CurrentGround = other.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == (int)Define.Layer.Ground)
        {
            OnGround = false;
        }
    }

    #endregion
}

