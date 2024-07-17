using System;
using System.Collections;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IMeleeWeapon
{
    #region Interface Properties

    public Action OnAttack { get; set; }
    [field:SerializeField]
    public float AttackDelay { get; private set; }

    #endregion

    [SerializeField] private float damage;
    [SerializeField] private float attackRange;
    private Collider2D hitBox;
    private Actor owner;

    private void Awake()
    {
        hitBox = Util.GetOrAddComponent<Collider2D>(gameObject);
        hitBox.enabled = false;
        gameObject.SetActive(false);
    }

    public void Init(Actor owner)
    {
        this.owner = owner;
        gameObject.SetActive(true);
    }

    public void Attack()
    {
        OnAttack?.Invoke();
        hitBox.enabled = true;
        StartCoroutine(DisableHitBox());
    }

    private IEnumerator DisableHitBox()
    {
        yield return new WaitForSeconds(0.1f);
        hitBox.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable target = collision.GetComponent<IDamageable>();
        if (target != null && !collision.CompareTag("Player"))
        {
            Vector2 dir = (collision.transform.position - transform.position).normalized;
            target.OnDamage(damage, transform.position, dir);
        }
    }
}
