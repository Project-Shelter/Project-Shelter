using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    #region Variables

    public Transform Tr { get; private set; }
    public Collider2D Coll { get; private set; }
    public MonsterStat Stat { get; private set; }
    public MonsterStateMachine StateMachine { get; private set; }

    #endregion

    private void Awake()
    {
        InitVariables();
    }

    private void InitVariables()
    {
        Tr = transform;
        Coll = Util.GetOrAddComponent<Collider2D>(gameObject);
        Stat = GetComponent<MonsterStat>();
        StateMachine = new MonsterStateMachine(this);
    }

}
