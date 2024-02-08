using UnityEngine;
using static MonsterStateManager;

public class MonsterStateChanger
{
    public static void ChangeState(MonsterStateManager manager)
    {
        ChangeFromAny(manager);
        switch (manager.currentState)
        {
            case MonsterState.Move:
                ChangeFromMove(manager);
                break;
            case MonsterState.Chase:
                ChangeFromChase(manager);
                break;
            case MonsterState.Attack:
                ChangeFromAttack(manager);
                break;
            case MonsterState.Injured:
                ChangeFromInjured(manager);
                break;
            case MonsterState.Die:
                ChangeFromDie(manager);
                break;
        }
    }

    private static void ChangeFromAny(MonsterStateManager manager)
    {
        if (manager.isHit)
        {
            manager.SetState(MonsterState.Injured);
        }
        if (manager.IsDead)
        {
            manager.SetState(MonsterState.Die);
        }
    }

    private static void ChangeFromMove(MonsterStateManager manager)
    {
        if (manager.CanChase)
        {
            manager.SetState(MonsterState.Chase);
        }
    }

    private static void ChangeFromChase(MonsterStateManager manager)
    {

        if (!manager.CanChase)
        {
            manager.SetState(MonsterState.Move);
        }

        if (manager.canAttack)
        {
            manager.SetState(MonsterState.Attack);
        }
    }

    private static void ChangeFromAttack(MonsterStateManager manager)
    {
        if (!manager.canAttack)
        {
            if (manager.CanChase)
            {
                manager.SetState(MonsterState.Chase);
            }
            else
            {
                manager.SetState(MonsterState.Move);
            }

        }
    }

    private static void ChangeFromInjured(MonsterStateManager manager)
    {
        if (!manager.isInjured)
        {
            if (manager.CanChase)
            {
                manager.SetState(MonsterState.Chase);
            }
            else
            {
                manager.SetState(MonsterState.Move);
            }
        }
    }

    private static void ChangeFromDie(MonsterStateManager manager)
    {

    }
}
