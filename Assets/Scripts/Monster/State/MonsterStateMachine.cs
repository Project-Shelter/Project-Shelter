using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine : StateMachine<MonsterStateMachine>
{
    public Monster Owner { get; private set; }

    public MonsterStateMachine(Monster owner)
    {
        Owner = owner;
        InitState();
    }

    protected virtual void InitState()
    { 
        AddState("Move", new MonsterMove(this));
        AddState("Chase", new MonsterChase(this));
        AddState("Attack", new MonsterAttack(this));
        AddState("ObjAttack", new MonsterObjAttack(this));
    }
}
