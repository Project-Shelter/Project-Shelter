using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    public enum Layer
    {
        Wall = 3,
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
