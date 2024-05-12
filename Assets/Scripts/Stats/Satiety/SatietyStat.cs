using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SatietyStat
{
    public Stat SatietyIndex { get; private set; } = new Stat();
    public Stat MinSatiety { get; private set; } = new Stat();
    public float Value { get; private set; }
    public Stat MaxSatiety { get; private set; } = new Stat();
    private readonly float TIME_BET_DECREASE = 3;

    public Dictionary<float, IBuff> DebuffsByBelow { get; private set; } = new Dictionary<float, IBuff>();
    public Dictionary<float, IBuff> BuffsByAbove { get; private set; } = new Dictionary<float, IBuff>();

    ActorStat stat;

    public SatietyStat(ActorStat stat, float minSatiety, float maxSatiety, float satietyIndex)
    {
        this.stat = stat;
        MinSatiety.baseValue = minSatiety;
        MaxSatiety.baseValue = maxSatiety;
        SatietyIndex.baseValue = satietyIndex;

        Value = MaxSatiety.GetValue();

        CoroutineHandler.StartStaticCoroutine(DecreaseSatietyCoroutine(Time.time, TIME_BET_DECREASE));
        InitSatietyBuffs();
    }

    void InitSatietyBuffs()
    {
        DebuffsByBelow[50] = new BleedingDebuff(stat, stat.hpDebuffValue.GetValue(), "HPDebuff");
        DebuffsByBelow[30] = new SlowDebuff(stat, stat.moveSpeedDebuffValue.GetValue(), "MoveSpeedDebuff");
        DebuffsByBelow[10] = new OverloadDebuff(stat, stat.overloadDebuffValue.GetValue(), "OverloadDebuff");
        DebuffsByBelow[0] = new DamageDecreaseDebuff(stat, stat.damageDebuffValue.GetValue(), "DamageDebuff");

        BuffsByAbove[90] = new HPRecoveryBuff(stat, stat.hpRecoveryBuffValue.GetValue(), "HPRecoveryBuff");

        foreach (float aboveSatiety in BuffsByAbove.Keys)
        {
            if (Value >= aboveSatiety)
            {
                IBuff buff = BuffsByAbove[aboveSatiety];
                stat.AddBuff(buff);
            }
        }
    }

    public void RestoreSatiety(float restoreAmount)
    {
        if (Value + restoreAmount > MaxSatiety.GetValue())
        {
            Value = MaxSatiety.GetValue();
        }
        else
        {
            Value += restoreAmount;
        }

        foreach (float belowSatiety in DebuffsByBelow.Keys)
        {
            if (Value > belowSatiety)
            {
                IBuff debuff = DebuffsByBelow[belowSatiety];
                stat.RemoveBuff(debuff);
            }
        }

        foreach (float aboveSatiety in BuffsByAbove.Keys)
        {
            if (Value >= aboveSatiety)
            {
                IBuff buff = BuffsByAbove[aboveSatiety];
                stat.AddBuff(buff);
            }
        }
    }

    public void DecreaseSatiety(float decreaseAmount)
    {
        Value -= decreaseAmount;
        if (Value <= MinSatiety.GetValue())
        {
            Value = MinSatiety.GetValue();
        }

        foreach (float belowSatiety in DebuffsByBelow.Keys)
        {
            if (Value <= belowSatiety)
            {
                IBuff debuff = DebuffsByBelow[belowSatiety];
                stat.AddBuff(debuff);
            }
        }

        foreach (float aboveSatiety in BuffsByAbove.Keys)
        {
            if (Value < aboveSatiety)
            {
                IBuff buff = BuffsByAbove[aboveSatiety];
                stat.RemoveBuff(buff);
            }
        }
    }

    private IEnumerator DecreaseSatietyCoroutine(float lastDecreaseTime, float timeBetDecrease)
    {
        while (true)
        {
            if (lastDecreaseTime + timeBetDecrease <= Time.time)
            {
                lastDecreaseTime = Time.time;
                DecreaseSatiety(SatietyIndex.GetValue());
            }

            yield return null;
        }
    }

}
