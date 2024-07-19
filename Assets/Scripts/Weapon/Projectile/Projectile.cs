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
    private Collider2D collider;
    private SpriteRenderer sprite;
    private ParticleSystem explosionEffect;
    private bool isExploded = false;

    private void Awake()
    {
        rigid = Util.GetOrAddComponent<Rigidbody2D>(gameObject);
        collider = Util.GetOrAddComponent<Collider2D>(gameObject);
        sprite = Util.GetOrAddComponent<SpriteRenderer>(gameObject);
        explosionEffect = Util.FindChild<ParticleSystem>(gameObject, "ExplosionEffect");
    }

    public void Launch(Vector2 dir, float range, float speed)
    {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        startPos = transform.position;
        direction = dir.normalized;
        rigid.velocity = direction * speed;
        this.range = range;
    }

    private void Update()
    {
        if (Vector2.Distance(startPos, transform.position) >= range)
        {
            Destroy(gameObject);
        }

        if(isExploded && !explosionEffect.IsAlive())
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
            Explode();
        }
        else if (collision.CompareTag("Wall"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        explosionEffect.Play();
        rigid.velocity = Vector2.zero;
        sprite.enabled = false;
        collider.enabled = false;
        isExploded = true;
    }
}
