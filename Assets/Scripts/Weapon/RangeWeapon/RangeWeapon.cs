using System;
using UnityEngine;

public class RangeWeapon : MonoBehaviour, IRangeWeapon
{
    #region Interface Properties

    public Action OnAttack { get; set; }
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
    [SerializeField] private Transform firePos;

    private ParticleSystem fireEffect;
    private ParticleSystemRenderer fireEffectRenderer;
    private Projectile projectilePrefab;
    private SpriteRenderer sprite;
    private Animator animator;
    private Actor owner;

    private void Awake()
    {
        fireEffect = Util.GetOrAddComponent<ParticleSystem>(firePos.gameObject);
        fireEffectRenderer = Util.GetOrAddComponent<ParticleSystemRenderer>(fireEffect.gameObject);
        projectilePrefab = Managers.Resources.Load<Projectile>("Prefabs/Weapon/Projectile");
        sprite = Util.GetOrAddComponent<SpriteRenderer>(gameObject);
        animator = Util.GetOrAddComponent<Animator>(gameObject);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if(owner != null)
        {
            animator.SetInteger("Direction", (int)owner.MoveBody.LookDir);
        }
    }

    public void Init(Actor owner)
    {
        this.owner = owner;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        owner.MoveBody.OnLookDirChanged += SetWeaponDirection;
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
}
