using System.Collections;
using UnityEngine;

public class HPRecoveryBuff : IBuff
{
    private ActorStat stat;

    private readonly float timeBetRecovery = 3.0f;
    private float currentTime;
    private float recoveryValue;

    private Coroutine hpRecoveryCoroutine;

    public string Tag { get; set; }
    public HPRecoveryBuff(ActorStat stat, float buffValue, string tag)
    {
        this.stat = stat;
        recoveryValue = buffValue;
        Tag = tag;
        currentTime = 0f;
    }

    public void TurnOn()
    {
        currentTime = Time.time;
        hpRecoveryCoroutine = CoroutineHandler.StartStaticCoroutine(HPRecoveryCoroutine(currentTime, timeBetRecovery));
    }

    public void TurnOff()
    {
        CoroutineHandler.StopStaticCoroutine(hpRecoveryCoroutine);
    }

    IEnumerator HPRecoveryCoroutine(float lastRecoveryTime, float timeBetRecovery)
    {

        while (true)
        {
            if (lastRecoveryTime + timeBetRecovery <= Time.time)
            {
                lastRecoveryTime = Time.time;
                stat.RestoreHP(recoveryValue);
            }

            yield return null;
        }
    }
}
