using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : MonoBehaviour
{
    #region Stat

    [Header("HP")]
    public float minHP;
    public float maxHP;
    public Stat stunTime;

    [Header("Move")]
    public MonsterMoveType moveType;
    public Stat dayMoveSpeed;
    public Stat nightMoveSpeed;

    public Vector3[] patrolMovePos;
    public float moveTime;


    [Header("Chase")]
    public Stat dayChaseSpeed;
    public Stat nightChaseSpeed;
    public int chasePatience = 5;
    public Stat chaseRadius;
    public Stat chaseHeight;

    [Header("Attack")]
    public Stat attackDamage;
    public Stat attackRange;
    public Stat attackDelay;

    [Header("Die")]
    public Stat deadBodyLiveTime;

    #endregion
}