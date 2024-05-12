using System.Collections;

public interface IHPStat : IDamageable, IRestoreable
{
    IEnumerator BleedingCoroutine(float lastBleedingTime, float timeBetBleeding);
}
