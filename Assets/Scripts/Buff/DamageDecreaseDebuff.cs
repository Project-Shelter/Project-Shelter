public class DamageDecreaseDebuff : IBuff
{
    private ActorStat stat;
    private float damageModifier;
    private float decreaseRatio;

    public string Tag { get; set; }

    public DamageDecreaseDebuff(ActorStat stat, float buffValue, string tag)
    {
        this.stat = stat;
        decreaseRatio = buffValue;
        Tag = tag;
    }

    public void TurnOn()
    {
        damageModifier = -stat.attackDamage.GetValue() * decreaseRatio;
        stat.attackDamage.AddModifier(damageModifier);
    }

    public void TurnOff()
    {
        stat.attackDamage.RemoveModifier(damageModifier);
    }

}
