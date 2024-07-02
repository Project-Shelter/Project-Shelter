using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float range;
    public int damage = 1;

    private Vector2 direction;
    private Vector3 startPos;
    private Rigidbody2D rigid;

    public void Launch(Vector2 dir, float range, float speed)
    {
        startPos = transform.position;
        direction = dir.normalized;
        rigid = Util.GetOrAddComponent<Rigidbody2D>(gameObject);
        rigid.velocity = direction * speed;
        this.range = range;
    }

    private void Update()
    {
        if (Vector2.Distance(startPos, transform.position) >= range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable target = collision.GetComponent<IDamageable>();
        if (target != null && !collision.CompareTag("Player"))
        {
            target.OnDamage(damage, transform.position, direction);
            Destroy(gameObject);
        }
    }
}
