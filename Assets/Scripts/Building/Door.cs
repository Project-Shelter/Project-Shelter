using System.Collections;
using UnityEngine;

// 건물 외부에서 바로 진입하는 Door
public class Door : MonoBehaviour, IPathway
{
    public Define.Layer linkedFloor;
    private GameObject roof; 
    
    private Entrance entrance;
    public Direction enterDirection;
    
    private void Awake()
    {
        InitVairables();
    }

    private void InitVairables()
    {
        roof = Util.FindChild(transform.parent.gameObject, "Roof");
        entrance = new Entrance(this, enterDirection);
    }

    public void PassEntrance(IMovable movable)
    {
        if (movable == (IMovable)ActorController.Instance.CurrentActor)
        {
            ActorController.Instance.CurrentActor.EnterBuilding(roof);
        }
        movable.ChangeFloor(linkedFloor);
    }

    public void PassExit(IMovable movable)
    {
        if (movable == (IMovable)ActorController.Instance.CurrentActor)
        {
            ActorController.Instance.CurrentActor.ExitBuilding();
        }
        movable.ChangeFloor(Define.Layer.Ground);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        entrance.OnTriggerExit2D(collision);
    }
}
