using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine : StateMachine<MonsterStateMachine>
{
    public Monster Owner { get; private set; }

    public MonsterStateMachine(Monster owner)
    {
        Owner = owner;
    }
}
