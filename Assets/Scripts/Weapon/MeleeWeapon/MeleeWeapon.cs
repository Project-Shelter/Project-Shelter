using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IMeleeWeapon
{
    [field:SerializeField]
    public float AttackDelay { get; private set; }
    [SerializeField] private float damage;
    [SerializeField] private float attackRange;

    private Collider2D hitBox;

    private void Awake()
    {
        hitBox = Util.GetOrAddComponent<Collider2D>(gameObject);
        hitBox.enabled = false;
    }

    public void Attack()
    {
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
