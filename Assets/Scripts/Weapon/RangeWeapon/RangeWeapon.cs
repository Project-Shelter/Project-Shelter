using ItemContainer;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : MonoBehaviour, IRangeWeapon
{
    public bool IsVisible { get; private set; }
    public Action<int, int> OnAmmoChanged { get; set; }
    public Action OnAttack { get; set; }

    // Weapon Info
    public float AttackDelay { get; private set; }
    public float ReloadDelay { get; private set; }
    private float damage;
    private float attackRange;
    private float projectileSpeed;
    public int MaxAmmo { get; private set; }
    private int currentAmmo;
    public int CurrentAmmo { 
        get { return currentAmmo; } 
        private set { currentAmmo = value; OnAmmoChanged?.Invoke(MaxAmmo, value); } 
    }
    private Transform firePos;

    private ParticleSystem fireEffect;
    private ParticleSystemRenderer fireEffectRenderer;
    private Projectile projectilePrefab;
    private SpriteRenderer sprite;
    private Animator animator;
    private Actor owner;

    private readonly int projectileID = 202006;
    List<ItemVO> availableAmmo = new();

    public void Init()
    {
        firePos = Util.FindChild<Transform>(gameObject, "FirePos");
        fireEffect = Util.GetOrAddComponent<ParticleSystem>(firePos.gameObject);
        fireEffectRenderer = Util.GetOrAddComponent<ParticleSystemRenderer>(fireEffect.gameObject);
        projectilePrefab = Managers.Resources.Load<Projectile>("Prefabs/Projectile/Projectile");
        sprite = Util.GetOrAddComponent<SpriteRenderer>(gameObject);
        animator = Util.GetOrAddComponent<Animator>(gameObject);
    }

    public void Active(Actor owner, ItemEffect weaponInfo)
    {
        this.owner = owner;
        gameObject.layer = owner.gameObject.layer;
        owner.MoveBody.OnLookDirChanged += SetWeaponDirection;
        SetWeaponDirection(owner.MoveBody.LookDir);

        AttackDelay = weaponInfo.Runtime;
        ReloadDelay = weaponInfo.AfterRuntime;
        damage = weaponInfo.Value;
        attackRange = weaponInfo.Range;
        projectileSpeed = 10f; // weaponInfo.ProjectileSpeed;
        MaxAmmo = 10; // weaponInfo.MaxAmmo;
    }

    public void SetVisibility(bool value)
    {
        IsVisible = value;
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
        if (CurrentAmmo <= 0 || availableAmmo.Count == 0)
        {
            return;
        }

        ItemVO ammo = null;
        foreach(ItemVO item in availableAmmo)
        {
            if(item.Count > 0)
            {
                ammo = item;
                break;
            }
        }

        ammo.Count--;
        CurrentAmmo--;

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
    }

    public void Reload()
    {
        int ammoCount = 0;
        for (int i = 1; i >= 0; i--)
        {
            foreach (ItemVO item in ItemDummyData.invenSlots[i].Values)
            {
                if (item.id == projectileID)
                {
                    availableAmmo.Add(item);
                    ammoCount += item.Count;
                    Action<int> reviewAmmo = (count) => {
                        if (count > 0) { return; }
                        int restAmmo = GetAvailableAmmoCount() - item.Count;
                        if (restAmmo < CurrentAmmo) CurrentAmmo = restAmmo; 
                    };
                    item.OnCountChanged -= reviewAmmo;
                    item.OnCountChanged += reviewAmmo;
                }
            }
        }

        if(ammoCount > 0 && ammoCount < MaxAmmo)
        {
            CurrentAmmo = ammoCount;
            return;
        }
        else if(ammoCount >= MaxAmmo) { CurrentAmmo = MaxAmmo; }
    }

    private int GetAvailableAmmoCount()
    {
        int ammoCount = 0;
        foreach (ItemVO item in availableAmmo)
        {
            ammoCount += item.Count;
        }
        return ammoCount;
    }

    private void SetWeaponDirection(Direction dir)
    {
        animator.SetInteger("Direction", (int)dir);
    }

    private void OnEnable()
    {
        if (owner != null)
        {
            animator.SetInteger("Direction", (int)owner.MoveBody.LookDir);
        }
    }

    public void OnDisable ()
    {
        if (owner != null)
        {
            owner.MoveBody.OnLookDirChanged -= SetWeaponDirection;
        }
    }
}
