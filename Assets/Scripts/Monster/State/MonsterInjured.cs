using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInjured : MonsterBaseState
{
    public MonsterInjured(in MonsterStateManager manager) : base(manager) { }

    private float hitTime;

    public override void OnStateEnter()
    {
        Manager.isHit = false;
        Manager.isInjured = true;
        Manager.Animator.SetTrigger("InjuredFront");
        Manager.Animator.SetFloat("MovingBlend", 0f);
        hitTime = Time.time;
    }

    public override void OnStateUpdate()
    {
        Manager.Move(Vector2.zero);
        if(hitTime + Manager.Stat.stunTime.GetValue() <= Time.time)
        {
            Manager.isInjured = false;
        }
    }

    public override void OnStateFixedUpdate()
    {

    }

    public override void OnStateExit()
    {

    }
}
