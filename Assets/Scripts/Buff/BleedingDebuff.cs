using UnityEngine;

public class BleedingDebuff : IBuff
{
    private ILivingEntity bleedingable;
    private float bleedingTick;

    public string Tag { get; set; }
    public BleedingDebuff(ILivingEntity bleedingable, float buffValue, string tag)
    {
        this.bleedingable = bleedingable;
        bleedingTick = buffValue;
        Tag = tag;
    }

    public void TurnOn()
    {
        bleedingable.Bleeding(bleedingTick);
    }

    public void TurnOff()
    {
        bleedingable.StopBleeding();
    }
}
