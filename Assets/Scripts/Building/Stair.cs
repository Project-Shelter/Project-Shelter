using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 건물 내부에 있는 계단
public class Stair : MonoBehaviour, IPathway
{
    private Define.Layer nowFloor;
    public Define.Layer linkedFloor;

    public Direction enterDirection;
    private Entrance entrance;

    private void Awake()
    {
        entrance = new Entrance(this, enterDirection);
        nowFloor = (Define.Layer)gameObject.layer;
    }

    public void PassEntrance(IMovable movable)
    {
        if (movable == (IMovable)ActorController.Instance.CurrentActor)
        {
            Camera.main.cullingMask |= 1 << (int)linkedFloor;
            if(nowFloor != Define.Layer.Ground) Camera.main.cullingMask &= ~(1 << (int)nowFloor);   
        }
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
