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

    private bool justNowAttack;
    private bool isAttacking;
    private bool afterAttacking;
    private Direction attackDir;

    private Vector3 swingVector;
    private Vector3 originRotation;
    private Vector3 attackRotation;
    private float rotateTime;

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

        justNowAttack = true;
        isAttacking = true;
        attackDir = owner.MoveBody.LookDir;

        swingVector = GetSwingVector(attackDir);
        rotateTime = 0;
    }

    public void AfterAttack()
    {
        hitBox.enabled = false;
        isAttacking = false;
        afterAttacking = true;

        swingVector *= -1;
        rotateTime = 0;
    }

    private Vector3 GetSwingVector(Direction dir)
    {
        swingVector = Vector3.zero;
        switch (dir)
        {
            case Direction.Up:
                swingVector = Vector3.left;
                break;
            case Direction.Down:
                swingVector = Vector3.right;
                break;
            case Direction.Left:
                swingVector = Vector3.forward;
                break;
            case Direction.Right:
                swingVector = Vector3.back;
                break;
        }

        return swingVector * 90;
    }

    public void EndAttack()
    {
        hitBox.enabled = false;
        isAttacking = false;
        afterAttacking = false;
    }

    private void SetWeaponDirection(Direction dir)
    {
        animator.SetInteger("Direction", (int)dir);
    }

    private void LateUpdate()
    {
        if (isAttacking)
        {
            if(justNowAttack)
            {
                justNowAttack = false;
                originRotation = transform.rotation.eulerAngles;
                attackRotation = swingVector + originRotation;
            }
            rotateTime += Time.deltaTime;
            float t = rotateTime / AttackDelay;

            transform.rotation = Quaternion.Lerp(Quaternion.Euler(originRotation), Quaternion.Euler(attackRotation), t);
        }
        else if (afterAttacking)
        {
            rotateTime += Time.deltaTime;
            float t = rotateTime / AfterAttackDelay;

            transform.rotation = Quaternion.Lerp(Quaternion.Euler(attackRotation), Quaternion.Euler(originRotation), t);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable target = collision.GetComponent<IDamageable>();
        if (target != null && !collision.CompareTag("Player"))
        {
            Vector2 targetCenter = collision.bounds.center;
            Vector2 hitPoint = hitBox.ClosestPoint(targetCenter);
            Vector2 hitNormal = (targetCenter - (Vector2)owner.Tr.position).normalized;
            target.OnDamage(damage, hitPoint, hitNormal, owner);
            Debug.Log(hitPoint);

            Vector2 onHitPos = targetCenter - hitNormal * 0.1f;

            onHitEffect.transform.position = hitPoint;
            onHitEffect.Play();
            Debug.Log(onHitEffect.transform.position);
        }
    }
}
