public class SlowDebuff : IBuff
{
    private Stat moveSpeed;
    private float moveSpeedModifier;
    private float slowRatio;

    public string Tag { get; set; }
    public SlowDebuff(Stat moveSpeed, float buffValue, string tag)
    {
        this.moveSpeed = moveSpeed;
        slowRatio = buffValue;
        Tag = tag;
    }

    public void TurnOn()
    {
        moveSpeedModifier = -moveSpeed.GetValue() * slowRatio;
        moveSpeed.AddModifier(moveSpeedModifier);
    }

    public void TurnOff()
    {
        moveSpeed.RemoveModifier(moveSpeedModifier);
    }
}
