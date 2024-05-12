public interface ILivingEntity : IDamageable, IRestoreable
{
    bool IsDead { get; }
    void Bleeding(float bleedingTick);
    void StopBleeding();
}
