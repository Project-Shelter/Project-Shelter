using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    public enum Layer
    {
        Wall = 3,

        Ground = 10,
        Floor1 = 11,
        Floor2 = 12,
        Floor3 = 13,

        Character = 21,
        Enemy = 22,
        EnemyTrigger = 23,
    }
    
    public enum UIEvent
    {
        Click,
        Drag,
    }
}
