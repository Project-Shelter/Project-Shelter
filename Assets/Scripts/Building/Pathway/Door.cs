using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// 건물 외부에서 바로 진입하는 Door
public class Door : MonoBehaviour, IPathway
{
    public Define.Layer linkedFloor;

    private List<IMovable> isPassedEntrance = new();
    private List<IMovable> isPassedExit = new();

    private GameObject roof;
    private void Awake()
    {
        roof = Util.FindChild(transform.parent.gameObject, "Roof");
    }

    public void PassEntrance(IMovable movable)
    {
        if (isPassedExit.Contains(movable))
        {
            isPassedExit.Remove(movable);
            EnterBuilding(movable);
            return;
        }

        if(isPassedEntrance.Contains(movable))
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
        Debug.Log("EnterBuilding");
        if (movable == (IMovable)ActorController.Instance.CurrentActor)
        {
            Camera.main.cullingMask |= 1 << (int)linkedFloor;
            roof.SetActive(false);
        }
        movable.ChangeFloor(linkedFloor);
    }

    private void ExitBuilding(IMovable movable)
    {
        if (movable == (IMovable)ActorController.Instance.CurrentActor)
        {
            if(linkedFloor != Define.Layer.Ground) Camera.main.cullingMask &= ~(1 << (int)linkedFloor);
            roof.SetActive(true);
        }
        movable.ChangeFloor(Define.Layer.Ground);
    }
}
