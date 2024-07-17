using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BreakableObject : MonoBehaviour, ILivingEntity
{
    #region Variables

    public Transform Tr { get; private set; }
    public Collider2D Coll { get; private set; }
    public Animator Anim { get; private set; }
    public ObjectStateMachine StateMachine { get; private set; }
    private LivingEntity livingEntity;
    public bool IsDead => livingEntity.IsDead;

    #endregion

    private void Awake()
    {
        InitVariables();
    }

    private void InitVariables()
    {
        Tr = transform;
        Coll = Util.GetOrAddComponent<Collider2D>(gameObject);
        Anim = Util.GetOrAddComponent<Animator>(gameObject);
        StateMachine = new ObjectStateMachine(this);
        livingEntity = new LivingEntity(0f, 30f);
    }

    private void Start()
    {
        livingEntity.OnDeath += () => 
        {
            gameObject.SetActive(false);
            NavMeshController.Instance.UpdateMesh(Agent.WithObjects);
        }; 
        StateMachine.Init("Idle");
    }

    private void Update()
    {
        StateMachine.StateUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.StateFixedUpdate();
    }

    #region ILivingEntity

    public void OnDamage(float damage, Vector2 hitPoint, Vector2 hitNormal)
    {
        livingEntity.OnDamage(damage, hitPoint, hitNormal);
    }

    public void Bleeding(float bleedingTick)
    {
        livingEntity.Bleeding(bleedingTick);
    }

    public void StopBleeding()
    {
        livingEntity.StopBleeding();
    }

    public void RestoreHP(float restoreHP)
    {
        livingEntity.RestoreHP(restoreHP);
    }

    public void AddOnDeath(Action action)
    {
        livingEntity.OnDeath += action;
    }

    public void RemoveOnDeath(Action action) 
    {
       livingEntity.OnDeath -= action;
    }

    #endregion
}
