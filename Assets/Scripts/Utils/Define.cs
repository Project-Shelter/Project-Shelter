using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    public enum Layer
    {
        Wall = 3,
        Character = 21,
        CanEnterLadderDown = 22,
        CanEnterLadderUp = 23,
        Ladder = 24,
        Ground = 25,
        Enemy = 26,
        EnemyTrigger = 27,
        CanEnterStairDown = 28,
        CanEnterStairUp = 29,
        Stair = 30,
    }
    
    public enum UIEvent
    {
        Click,
        Drag,
    }
}
