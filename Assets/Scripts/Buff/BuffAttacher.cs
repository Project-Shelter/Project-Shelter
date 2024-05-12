using System;
using System.Collections.Generic;

public class BuffAttacher
{
    private ILivingEntity target;
    public Dictionary<string, IBuff> ActivedBuffs { get; private set; } = new();
    public Action<string> AddBuffAction = null;
    public Action<string> RemoveBuffAction = null;

    public BuffAttacher(ILivingEntity target)
    {
        this.target = target;
    }

    public void AddBuff(IBuff buff)
    {
        if (target.IsDead) return;

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
}
