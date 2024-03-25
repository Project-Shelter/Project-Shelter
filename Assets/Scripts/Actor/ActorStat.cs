using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Actor의 Stat
//수치 조정 용도로 MonoBehaviour 부착. 추후 테스트 종료 시 제거. -> Actor 스크립트에서도 작업 필요
public class ActorStat : MonoBehaviour
{
    #region Stat

    [Header("HP")]
    public readonly double debuffHPRatio = 0.8;
    public Stat minHP;
    public Stat maxHP;
    public float[] AttackedChances { get; private set; }
    public Stat headAttackedChance;
    public Stat trunkAttackedChance;
    public Stat legAttackedChance;
    public Stat armAttackedChance;

    [Header("Move")]
    public Stat moveSpeed;
    public Stat dashCoolTime;
    public Stat dashTime;
    public Stat dashSpeed;

    [Header("Attack")]
    public Stat attackDamage;

    [Header("Satiety")]
    public Stat minSatiety;
    public Stat maxSatiety;
    public Stat satietyIndex;

    [Header("Buff Value")]
    public Stat hpDebuffVal;
    public Stat slowDebuffVal;
    public Stat overloadDebuffVal;
    public Stat damageDebuffVal;
    public Stat collidingEnemyDebuffVal;
    public Stat hpRecoveryBuffVal;
    public Stat bleedingDebuffVal;

    #endregion

    private void Awake()
    {
        InitStat();
    }

    // 변동 여지가 있는 초기화 수치는 추후 작성 or 데이터로 받아옴
    private void InitStat()
    {
        AttackedChances = new float[(int)AttackedPart.Normal];
        AttackedChances[0] = headAttackedChance.GetValue();
        AttackedChances[1] = AttackedChances[0] + trunkAttackedChance.GetValue();
        AttackedChances[2] = AttackedChances[1] + legAttackedChance.GetValue();
        AttackedChances[3] = AttackedChances[2] + armAttackedChance.GetValue();
    }
}
