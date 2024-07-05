using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : MonoBehaviour, IRangeWeapon
{
    #region Interface Properties

    [field:SerializeField]
    public float AttackDelay { get; private set; }
    [field: SerializeField]
    public float ReloadDelay { get; private set; }
    public bool CanReload => currentAmmo < maxAmmo;
    public bool HasToBeReload => currentAmmo <= 0;

    #endregion

    // 나중에는 json으로 데이터를 불러올 것.
    [SerializeField] private int damage;
    [SerializeField] private float attackRange;
    [SerializeField] private float projectileSpeed;

    [SerializeField] private int maxAmmo;
    [SerializeField] private int currentAmmo;

    private Projectile projectilePrefab;

    private void Awake()
    {
        enabled = false;
        projectilePrefab = Managers.Resources.Load<Projectile>("Prefabs/Weapon/Projectile");
    }

    public void Attack()
    {
        if (HasToBeReload)
        {
            return;
        }
        Projectile projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        projectile.gameObject.layer = transform.gameObject.layer;

        Vector2 mousePos = InputHandler.MousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 dir = (worldPos - (Vector2)transform.position).normalized;
         
        projectile.Launch(dir, attackRange, projectileSpeed);
        currentAmmo--;
    }

    public void Reload()
    {
        // 인벤에서 총알 빠져나가는 작업 필요
        currentAmmo = maxAmmo;
    }
}
