public class OverloadDebuff : IBuff
{
    private Stat stat;
    private float moveSpeedModifier;
    private float ladderSpeedModifier;

    public string Tag { get; set; }

    public OverloadDebuff(Stat stat, float buffValue, string tag)
    {
        this.stat = stat;
        Tag = tag;
    }

    public void TurnOn()
    {
        // 최대 무게 과적 감소
    }

    public void TurnOff()
    {
        // 최대 무게 과적 감소 해제
    }
}
