using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : MonoBehaviour, IHPStat
{
    [field: SerializeField] public Vector2 ViewDistance { get; private set; }

    #region HP

    public float minHP;
    public MonsterHPStat HP;
    public float maxHP;
    public Stat stunTime;
    [field: SerializeField] public ParticleSystem OnDamageParticle { get; private set; }

    #endregion

    #region Move

    public enum MoveType
    {
        Idle,
        Patrol,
        Random,
    }
    [Header("Move")]
    public MoveType initialMoveType = MoveType.Idle;
    public Stat dayMoveSpeed;
    public Stat nightMoveSpeed;

    private float moveSpeedModifier;

    public float MoveSpeed
    {
        get
        {
            if (DayNight.Instance.isDay)
            {
                return dayMoveSpeed.GetValue();
            }
            else
            {
                return nightMoveSpeed.GetValue();
            }
        }
    }

    [Space]
    public Transform[] patrolPoints;

    [Space]
    public Stat randomMoveDistance;

    #endregion

    #region Chase

    [Header("Chase")]
    public Stat dayChaseSpeed;
    public Stat nightChaseSpeed;
    public float ChaseSpeed
    {
        get
        {
            if (DayNight.Instance.isDay)
            {
                return dayChaseSpeed.GetValue();
            }
            else
            {
                return nightChaseSpeed.GetValue();
            }
        }

    }
    public int chasePoint = 5;
    public Stat chaseRadius;
    public Stat chaseHeight;

    #endregion

    #region Attack

    [Header("Attack")]
    public Stat attackDamage;
    public Stat attackRange;
    [field: SerializeField] public Vector2 AttackPoint { get; private set; }

    #endregion

    #region Die

    [Header("Die")]
    public Stat deadBodyLiveTime;
    public bool IsDead
    {
        get
        {
            return HP.IsDead;
        }
    }

    #endregion

    private void Awake()
    {
        InitStat();
    }

    private void InitStat()
    {
        HP = new MonsterHPStat(this, minHP, maxHP);
        moveSpeedModifier = Random.Range(-0.4f, 0.4f);
        dayMoveSpeed.AddModifier(moveSpeedModifier); nightMoveSpeed.AddModifier(moveSpeedModifier);
        dayChaseSpeed.AddModifier(moveSpeedModifier); nightChaseSpeed.AddModifier(moveSpeedModifier);
    }

    public IEnumerator BleedingCoroutine(float lastBleedingTime, float timeBetBleeding)
    {
        return HP.BleedingCoroutine(lastBleedingTime, timeBetBleeding);
    }

    public void OnDamage(float damage, Vector2 hitPoint, Vector2 hitNormal)
    {
        HP.OnDamage(damage, hitPoint, hitNormal);
    }

    public void RestoreHP(float restoreHP)
    {
        HP.RestoreHP(restoreHP);
    }
}
