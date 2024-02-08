using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDie : MonsterBaseState
{
    public MonsterDie(in MonsterStateManager manager) : base(manager) { }

    private float timeAfterDead;

    public override void OnStateEnter()
    {
        timeAfterDead = Time.time;
        Manager.Animator.SetBool("IsDead", true);
    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateFixedUpdate()
    {
        Manager.Move(Vector2.zero);
        if (timeAfterDead + Manager.Stat.deadBodyLiveTime.GetValue() <= Time.time)
        {
            Manager.gameObject.SetActive(false);
        }
    }

    public override void OnStateExit()
    {
        Manager.Animator.SetBool("IsDead", false);
    }
}