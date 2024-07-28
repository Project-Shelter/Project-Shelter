using ItemContainer;
using System;
using UnityEngine;

public class RangeWeapon : MonoBehaviour, IRangeWeapon
{
    public bool IsActived { get; private set; }
    public Action OnAttack { get; set; }
    public bool CanReload => currentAmmo < maxAmmo;
    public bool HasToBeReload => currentAmmo <= 0;

    // Weapon Info
    public float AttackDelay { get; private set; }
    public float ReloadDelay { get; private set; }
    private float damage;
    private float attackRange;
    private float projectileSpeed;
    private int maxAmmo;

    private int currentAmmo;
    private Transform firePos;

    private ParticleSystem fireEffect;
    private ParticleSystemRenderer fireEffectRenderer;
    private Projectile projectilePrefab;
    private SpriteRenderer sprite;
    private Animator animator;
    private Actor owner;

    private void Awake()
    {
        firePos = Util.FindChild<Transform>(gameObject, "FirePos");
        fireEffect = Util.GetOrAddComponent<ParticleSystem>(firePos.gameObject);
        fireEffectRenderer = Util.GetOrAddComponent<ParticleSystemRenderer>(fireEffect.gameObject);
        projectilePrefab = Managers.Resources.Load<Projectile>("Prefabs/Weapon/Projectile");
        sprite = Util.GetOrAddComponent<SpriteRenderer>(gameObject);
        animator = Util.GetOrAddComponent<Animator>(gameObject);
        SetActive(false);
    }

    private void OnEnable()
    {
        if(owner != null)
        {
            animator.SetInteger("Direction", (int)owner.MoveBody.LookDir);
        }
    }

    public void Init(Actor owner, ItemEffect weaponInfo)
    {
        this.owner = owner;
        owner.MoveBody.OnLookDirChanged += SetWeaponDirection;
        SetWeaponDirection(owner.MoveBody.LookDir);

        AttackDelay = weaponInfo.Runtime;
        ReloadDelay = weaponInfo.AfterRuntime;
        damage = weaponInfo.Value;
        attackRange = weaponInfo.Range;
        projectileSpeed = 10f; // weaponInfo.ProjectileSpeed;
        maxAmmo = 10; // weaponInfo.MaxAmmo;

        SetActive(true);
    }

    public void SetActive(bool value)
    {
        IsActived = value;
        if (value)
        {
            sprite.enabled = true;
        }
        else
        {
            sprite.enabled = false;
        }
    }

    public void Attack()
    {
        if (HasToBeReload)
        {
            return;
        }
        OnAttack?.Invoke();
        Projectile projectile = Instantiate(projectilePrefab, firePos.position, firePos.rotation);
        projectile.gameObject.layer = owner.gameObject.layer;

        Vector2 mousePos = InputHandler.MousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 dir = (worldPos - (Vector2)transform.position).normalized;

        fireEffectRenderer.sortingLayerID = sprite.sortingLayerID;
        fireEffectRenderer.sortingOrder = sprite.sortingOrder;
        fireEffect.Play();
        projectile.Launch(dir, attackRange, projectileSpeed, owner);
        currentAmmo--;
    }

    public void Reload()
    {
        // 인벤에서 총알 빠져나가는 작업 필요
        currentAmmo = maxAmmo;
    }

    private void SetWeaponDirection(Direction dir)
    {
        animator.SetInteger("Direction", (int)dir);
    }
    public void OnDestroy()
    {
        owner.MoveBody.OnLookDirChanged -= SetWeaponDirection;
    }
}
