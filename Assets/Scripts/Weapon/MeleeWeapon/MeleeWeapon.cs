using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IMeleeWeapon
{
    #region Interface Properties

    public bool IsActived { get; private set; }
    public Action OnAttack { get; set; }
    [field:SerializeField]
    public float AttackDelay { get; private set; }
    [field:SerializeField]
    public float AfterAttackDelay { get; private set; }

    #endregion

    [SerializeField] private float damage;
    [SerializeField] private float attackRange;

    private Animator animator;
    private SpriteRenderer sprite;
    private Collider2D hitBox;
    private Actor owner;
    private ParticleSystem swingEffect;
    private ParticleSystem onHitEffect;
    private bool isAttacking;
    private bool isAttacked;
    private Direction attackDir;

    private float angle;
    private float targetAngle;
    private float rotateByTick;

    private float rotateTime;

    private List<IDamageable> attackedEntities;

    private void Awake()
    {
        animator = Util.GetOrAddComponent<Animator>(gameObject);
        sprite = Util.GetOrAddComponent<SpriteRenderer>(gameObject);
        hitBox = Util.GetOrAddComponent<Collider2D>(gameObject);
        swingEffect = Util.FindChild<ParticleSystem>(gameObject, "SwingEffect");
        onHitEffect = Util.FindChild<ParticleSystem>(gameObject, "OnHitEffect");
        var effectMain = swingEffect.main;
        effectMain.duration = AttackDelay;
        hitBox.enabled = false;
        SetActive(false);
    }

    public void Init(Actor owner)
    {
        this.owner = owner;
        owner.MoveBody.OnLookDirChanged += SetWeaponDirection;
        attackedEntities = new List<IDamageable>();
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
        OnAttack?.Invoke();
        swingEffect.Play();
        hitBox.enabled = true;
        isAttacking = true;
        isAttacked = false;
        attackDir = owner.MoveBody.LookDir;
        attackedEntities.Clear();

        float rotateSign = 0;
        switch (attackDir)
        {
            case Direction.Up:

                break;
            case Direction.Down:

                break;
            case Direction.Left:

                break;
            case Direction.Right:
                rotateSign = -1;
                break;
        }
        angle = transform.rotation.eulerAngles.z;
        angle = (angle > 180) ? angle - 360 : angle;
        targetAngle = angle + 90 * rotateSign;
        rotateTime = 0;
    }

    public void EndAttack()
    {
        hitBox.enabled = false;
        isAttacking = false;
        isAttacked = true;

        float rotateSign = 0;
        switch (attackDir)
        {
            case Direction.Up:

                break;
            case Direction.Down:

                break;
            case Direction.Left:

                break;
            case Direction.Right:
                rotateSign = 1;
                break;
        }
        angle = transform.rotation.eulerAngles.z;
        angle = (angle > 180) ? angle - 360 : angle;
        targetAngle = angle + 90 * rotateSign;
        rotateTime = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable target = collision.GetComponent<IDamageable>();
        if (target != null && !collision.CompareTag("Player"))
        {
            attackedEntities.Add(target);
            Vector2 targetCenter = collision.bounds.center;
            Vector2 hitPoint = hitBox.ClosestPoint(targetCenter);
            Vector2 hitNormal = (targetCenter - (Vector2)transform.position).normalized;
            target.OnDamage(damage, hitPoint, hitNormal, owner);
            Debug.Log(hitPoint);
            onHitEffect.transform.position = hitPoint;
            onHitEffect.Play();
        }
    }

    private void SetWeaponDirection(Direction dir)
    {
        animator.SetInteger("Direction", (int)dir);
    }

    private void LateUpdate()
    {
        if (isAttacking)
        { 
            rotateTime += Time.deltaTime;
            rotateByTick = 90 / (AttackDelay * 0.7f) * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.MoveTowards(angle, targetAngle, rotateByTick));
            angle = transform.rotation.eulerAngles.z;
            angle = (angle > 180) ? angle - 360 : angle;
        }
        if(isAttacked)
        {
            rotateTime += Time.deltaTime;
            rotateByTick = 90 / AfterAttackDelay * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.MoveTowards(angle, targetAngle, rotateByTick));
            angle = transform.rotation.eulerAngles.z;
            angle = (angle > 180) ? angle - 360 : angle;
            if (angle == targetAngle)
            {
                isAttacked = false;
                Debug.Log(rotateTime);
            }

        }
    }
}
