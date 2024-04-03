using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : MonoBehaviour
{
    public enum MoveType
    {
        Idle,
        Patrol,
        Random,
    }

    #region Stat

    [Header("HP")]
    public float minHP;
    public float maxHP;
    public Stat stunTime;

    [Header("Move")]
    public MoveType initialMoveType = MoveType.Idle;
    public Stat dayMoveSpeed;
    public Stat nightMoveSpeed;

    [Header("Chase")]
    public Stat dayChaseSpeed;
    public Stat nightChaseSpeed;
    public int chasePoint = 5;
    public Stat chaseRadius;
    public Stat chaseHeight;

    [Header("Attack")]
    public Stat attackDamage;
    public Stat attackRange;

    [Header("Die")]
    public Stat deadBodyLiveTime;

    #endregion
}