using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Actor의 Stat
//수치 조정 용도로 MonoBehaviour 부착. 추후 테스트 종료 시 제거. -> Actor 스크립트에서도 작업 필요
public class ActorStat : MonoBehaviour, IHPStat
{
    #region Stat
    [SerializeField] private bool isHumanPlayer = false;
    [field: SerializeField] public ParticleSystem ActorChangeEffect { get; private set; }

    [Header("HP")]
    private PlayerHPStat hp;
    public float[] AttackedChances { get; private set; }
    public readonly double DEBUFF_HP_RATIO = 0.8;

    [SerializeField] private float minHP = 0f;
    public float HP { get { return hp.HP; } }
    [SerializeField] private float maxHP = 100f;
    public Stat headAttackedChance;
    public Stat trunkAttackedChance;
    public Stat legAttackedChance;
    public Stat armAttackedChance;

    [field: SerializeField] public ParticleSystem OnDamageEffect { get; private set; }

    public bool IsDead { get { return hp.IsDead; } }
    public Action<PlayerHPStat.AttackedPart> AttackedAction = null;

    [Header("Move")]
    public Stat moveSpeed;
    public Stat moveOnLadderSpeed;

    [Header("Attack")] 
    public Stat attackSpeed;
    public Stat attackDamage;
    public Stat attackRange;
    [field: SerializeField] public Vector2 AttackPoint { get; private set; }

    [Header("Satiety")]
    public SatietyStat satiety;

    [SerializeField] private float minSatiety;
    [SerializeField] private float maxSatiety;
    [SerializeField] private float satietyIndex;

    #endregion

    #region Buff/Debuff

    public Dictionary<string, IBuff> ActivedBuffs { get; private set; } = new();

    [Header("Buff Value")]
    public Stat hpDebuffValue;
    public SlowDebuff CollidingEnemyDebuff { get; private set; }
    public Stat moveSpeedDebuffValue;
    public Stat overloadDebuffValue;
    public Stat damageDebuffValue;
    public Stat collidingEnemyDebuffValue;
    public Stat hpRecoveryBuffValue;
    public Stat bleedingDebuffValue;

    public Action<string> AddBuffAction = null;
    public Action<string> RemoveBuffAction = null;

    public void InitBuffs()
    {
        CollidingEnemyDebuff = new SlowDebuff(this, collidingEnemyDebuffValue.GetValue(), "CollidingEnemyDebuff");
    }

    public void AddBuff(IBuff buff)
    {
        if (IsDead) return;
        if (buff != null && !ActivedBuffs.ContainsKey(buff.Tag))
        {
            ActivedBuffs.Add(buff.Tag, buff);
            AddBuffAction?.Invoke(buff.Tag);
            buff.TurnOn();
        }
    }

    public void RemoveBuff(IBuff buff)
    {
        if (ActivedBuffs.ContainsKey(buff.Tag))
        {
            ActivedBuffs[buff.Tag].TurnOff();
            ActivedBuffs.Remove(buff.Tag);
            RemoveBuffAction?.Invoke(buff.Tag);
        }
    }

    #endregion

    private void Awake()
    {
        InitStat();
        InitBuffs();
    }

    // 변동 여지가 있는 초기화 수치는 추후 작성 or 데이터로 받아옴
    public void InitStat()
    {
        hp = new PlayerHPStat(this, minHP, maxHP);
        hp.OnDeath += () => { 
            foreach (var buff in ActivedBuffs) 
            {
                buff.Value.TurnOff();
                RemoveBuffAction?.Invoke(buff.Value.Tag);
            } 
        };
        hp.OnDeath += () => ActivedBuffs.Clear();
        if (isHumanPlayer)
        {
            // 게임 오버 처리
            hp.OnDeath += () => Debug.Log("GameOver!");
        }

        AttackedChances = new float[(int)PlayerHPStat.AttackedPart.Normal];
        AttackedChances[0] = headAttackedChance.GetValue();
        AttackedChances[1] = AttackedChances[0] + trunkAttackedChance.GetValue();
        AttackedChances[2] = AttackedChances[1] + legAttackedChance.GetValue();
        AttackedChances[3] = AttackedChances[2] + armAttackedChance.GetValue();

        satiety = new SatietyStat(this, minSatiety, maxSatiety, satietyIndex);
    }

    #region PlayerHP Composite

    public void OnDamage(float damage, Vector2 hitPoint, Vector2 hitNormal)
    {
        hp.OnDamage(damage, hitPoint, hitNormal);
        if (hp.nowAttackedPart != PlayerHPStat.AttackedPart.Normal)
        {
            AttackedAction?.Invoke(hp.nowAttackedPart);
        }
    }

    public void RestoreHP(float restoreHP)
    {
        hp.RestoreHP(restoreHP);
    }

    public IEnumerator BleedingCoroutine(float lastBleedingTime, float timeBetBleeding)
    {
        return hp.BleedingCoroutine(lastBleedingTime, timeBetBleeding);
    }

    #endregion
}
