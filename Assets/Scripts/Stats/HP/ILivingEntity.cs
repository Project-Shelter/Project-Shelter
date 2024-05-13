using UnityEngine;

public interface ILivingEntity : IDamageable, IRestoreable
{
    bool IsDead { get; }
    Collider2D Coll { get; }
    void Bleeding(float bleedingTick);
    void StopBleeding();
}
