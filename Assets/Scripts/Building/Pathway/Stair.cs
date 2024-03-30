using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// 건물 내부에 있는 계단
public class Stair : MonoBehaviour, IPathway
{
    public Define.Layer linkedFloor;

    private List<IMovable> isPassedEntrance = new();
    private List<IMovable> isPassedExit = new();

    private TilemapRenderer roofRenderer;
    private void Awake()
    {
        roofRenderer = Util.FindChild<TilemapRenderer>(transform.parent.gameObject, "Roof");
    }

    public void PassEntrance(IMovable movable)
    {
        if (isPassedExit.Contains(movable))
        {
            isPassedExit.Remove(movable);
            EnterBuilding(movable);
            return;
        }

        if (isPassedEntrance.Contains(movable))
        {
            isPassedEntrance.Remove(movable);
            return;
        }

        isPassedEntrance.Add(movable);
    }

    public void PassExit(IMovable movable)
    {
        if (isPassedEntrance.Contains(movable))
        {
            isPassedEntrance.Remove(movable);
            ExitBuilding(movable);
            return;
        }

        if (isPassedExit.Contains(movable))
        {
            isPassedExit.Remove(movable);
            return;
        }

        isPassedExit.Add(movable);
    }

    private void EnterBuilding(IMovable movable)
    {
        movable.ChangeFloor(linkedFloor);
        if (movable == (IMovable)ActorController.Instance.CurrentActor)
        {
            Camera.main.cullingMask |= 1 << (int)linkedFloor;
            roofRenderer.enabled = false;
        }
    }

    private void ExitBuilding(IMovable movable)
    {
        if (movable == (IMovable)ActorController.Instance.CurrentActor)
        {
            if (linkedFloor != Define.Layer.Ground) Camera.main.cullingMask &= ~(1 << (int)linkedFloor);
            roofRenderer.enabled = true;
        }
        movable.ChangeFloor(Define.Layer.Ground);
    }
}
