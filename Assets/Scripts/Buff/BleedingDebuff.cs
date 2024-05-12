using UnityEngine;

public class BleedingDebuff : IBuff
{
    private IHPStat bleeding;
    private Coroutine bleedingCoroutine;

    private float timeBetBleeding;
    private float currentTime;

    public string Tag { get; set; }

    public BleedingDebuff(IHPStat bleedingable, float buffValue, string tag)
    {
        bleeding = bleedingable;
        timeBetBleeding = buffValue;
        Tag = tag;
        currentTime = 0f;
    }

    public void TurnOn()
    {
        currentTime = Time.time;
        bleedingCoroutine = CoroutineHandler.StartStaticCoroutine(bleeding.BleedingCoroutine(currentTime, timeBetBleeding));
    }

    public void TurnOff()
    {
        CoroutineHandler.StopStaticCoroutine(bleedingCoroutine);
    }
}
