public class DamageDecreaseDebuff : IBuff
{
    private Stat damage;
    private float damageModifier;
    private float decreaseRatio;

    public string Tag { get; set; }

    public DamageDecreaseDebuff(Stat damage, float buffValue, string tag)
    {
        this.damage = damage;
        decreaseRatio = buffValue;
        Tag = tag;
    }

    public void TurnOn()
    {
        damageModifier = -damage.GetValue() * decreaseRatio;
        damage.AddModifier(damageModifier);
    }

    public void TurnOff()
    {
        damage.RemoveModifier(damageModifier);
    }

}
