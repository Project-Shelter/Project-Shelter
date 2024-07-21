using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IMeleeWeapon
{
    #region Interface Properties

    public bool IsActived { get; private set; }
    public Action OnAttack { get; set; }
    [field:SerializeField]
    public float AttackDelay { get; private set; }

    #endregion

    [SerializeField] private float damage;
    [SerializeField] private float attackRange;

    private Animator animator;
    private SpriteRenderer sprite;
    private Collider2D hitBox;
    private Actor owner;
    private Coroutine rotateCoroutine;

    private void Awake()
    {
        animator = Util.GetOrAddComponent<Animator>(gameObject);
        sprite = Util.GetOrAddComponent<SpriteRenderer>(gameObject);
        hitBox = Util.GetOrAddComponent<Collider2D>(gameObject);
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
        hitBox.enabled = true;
        animator.SetBool("Attack", true);
        rotateCoroutine = owner.StartCoroutine(RotateWhenAttacks(owner.MoveBody.LookDir));
    }

    public void EndAttack()
    {
        hitBox.enabled = false;
        animator.SetBool("Attack", false);
        owner.StopCoroutine(rotateCoroutine);
        transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable target = collision.GetComponent<IDamageable>();
        if (target != null && !collision.CompareTag("Player"))
        {
            Vector2 dir = (collision.transform.position - transform.position).normalized;
            target.OnDamage(damage, transform.position, dir, owner);
        }
    }

    private void SetWeaponDirection(Direction dir)
    {
        animator.SetInteger("Direction", (int)dir);
    }

    private IEnumerator RotateWhenAttacks(Direction dir)
    {
        float fallDir;
        if(dir == Direction.Right)
        {
            fallDir = 1;
        }
        else
        {
            fallDir = -1;
        }

        float angle = transform.rotation.eulerAngles.y;
        float targetAngle = 180 * fallDir;
        float rotateByTick = Mathf.DeltaAngle(angle, targetAngle) / (AttackDelay / Time.fixedDeltaTime);
        Debug.Log(Mathf.DeltaAngle(angle, targetAngle));

        while (true)
        {
            transform.rotation =
                Quaternion.Euler(0, Mathf.MoveTowards(angle, targetAngle, rotateByTick), transform.rotation.eulerAngles.z);
            angle = transform.rotation.eulerAngles.y;
            angle = (angle > 180) ? angle - 360 : angle;
            yield return Time.fixedDeltaTime;
        }
    }
}
