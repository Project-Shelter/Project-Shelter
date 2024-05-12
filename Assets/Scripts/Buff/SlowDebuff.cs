public class SlowDebuff : IBuff
{
    private ActorStat stat;
    private float moveSpeedModifier;
    private float ladderSpeedModifier;
    private float slowRatio;

    public string Tag { get; set; }
    public SlowDebuff(ActorStat stat, float buffValue, string tag)
    {
        this.stat = stat;
        slowRatio = buffValue;
        Tag = tag;
    }

    public void TurnOn()
    {
        moveSpeedModifier = -stat.moveSpeed.GetValue() * slowRatio;
        ladderSpeedModifier = -stat.moveOnLadderSpeed.GetValue() * slowRatio;

        stat.moveSpeed.AddModifier(moveSpeedModifier);
        stat.moveOnLadderSpeed.AddModifier(ladderSpeedModifier);
    }

    public void TurnOff()
    {
        stat.moveSpeed.RemoveModifier(moveSpeedModifier);
        stat.moveOnLadderSpeed.RemoveModifier(ladderSpeedModifier);
    }
}
