using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 건물 내부에 있는 계단
public class Stair : MonoBehaviour, IPathway
{
    private Define.Layer nowFloor;
    public Define.Layer linkedFloor;

    private Entrance entrance;
    public Direction enterDirection;

    private void Awake()
    {
        InitVariables();
    }

    private void InitVariables()
    {
        nowFloor = (Define.Layer)gameObject.layer;
        entrance = new Entrance(this, enterDirection);
    }

    public void PassEntrance(IMovable movable)
    {
        movable.ChangeFloor(linkedFloor);
    }

    public void PassExit(IMovable movable)
    {
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        entrance.OnTriggerExit2D(collision);
    }
}
