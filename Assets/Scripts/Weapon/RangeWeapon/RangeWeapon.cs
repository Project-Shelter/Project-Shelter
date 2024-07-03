using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : IRangeWeapon
{
    #region Interface Properties

    public float AttackDelay { get; private set; }
    public float ReloadDelay { get; private set; }
    public bool CanReload => currentAmmo < maxAmmo;
    public bool HasToBeReload => currentAmmo <= 0;

    #endregion

    private int damage;
    private float attackRange;
    private float projectileSpeed;

    private int maxAmmo;
    private int currentAmmo;

    private Transform socket;
    private Projectile projectilePrefab;

    // 나중에는 json으로 데이터를 불러올 것. 지금은 생성자를 통해 데이터를 받아옴
    public RangeWeapon(Transform socket, int damage, float attackDelay, float attackRange, float reloadDelay, float projectileSpeed, int maxAmmo, int currentAmmo)
    {
        AttackDelay = attackDelay;
        this.socket = socket;
        this.damage = damage;
        this.attackRange = attackRange;
        ReloadDelay = reloadDelay;
        this.projectileSpeed = projectileSpeed;
        this.maxAmmo = maxAmmo;
        this.currentAmmo = currentAmmo;

        projectilePrefab = Managers.Resources.Load<Projectile>("Prefabs/Projectile");
    }

    public void Attack()
    {
        if (HasToBeReload)
        {
            return;
        }
        Projectile projectile = Object.Instantiate(projectilePrefab, socket.position, socket.rotation);
        projectile.gameObject.layer = socket.gameObject.layer;

        Vector2 mousePos = InputHandler.MousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 dir = (worldPos - (Vector2)socket.position).normalized;
         
        projectile.Launch(dir, attackRange, projectileSpeed);
        currentAmmo--;
    }

    public void Reload()
    {
        // 인벤에서 총알 빠져나가는 작업 필요
        currentAmmo = maxAmmo;
    }
}
