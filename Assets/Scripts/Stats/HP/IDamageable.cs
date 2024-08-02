using UnityEngine;

// 데미지를 입을 수 있는 타입들이 공통적으로 가져야 하는 인터페이스
public interface IDamageable
{
    void OnDamage(float damage, Vector2 hitPoint, Vector2 hitNormal, ILivingEntity attacker);
}
