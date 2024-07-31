using System;
using System.Collections.Generic;
using UnityEngine;

public partial class Actor : MonoBehaviour, ILivingEntity, IMovable
{
    #region CollisionVariables

    public IInteractable Interactable { get; private set; }
    public BreakableObject Concealment { get; set; }
    private int countCollidingEnemy;
    private SlowDebuff collidingEnemyDebuff;
    private GameObject roof;

    #endregion

    private void InitCollisionVariables()
    {
        countCollidingEnemy = 0;
        collidingEnemyDebuff = new SlowDebuff(Stat.moveSpeed, Stat.collidingEnemyDebuffVal.GetValue(), "CollidingEnemyDebuff");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print(other.gameObject.name);
        if (other.gameObject.layer == (int)Define.Layer.EnemyTrigger)
        {
            countCollidingEnemy++;
            Buff.AddBuff(collidingEnemyDebuff);
        }
        if(other.TryGetComponent(out IInteractable interactable))
        {
            interactable.ShowGuide(true);
            Interactable = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == (int)Define.Layer.EnemyTrigger)
        {
            countCollidingEnemy--;
            if (countCollidingEnemy == 0)
            {
                Buff.RemoveBuff(collidingEnemyDebuff);
            }
        }
        if (other.TryGetComponent(out IInteractable interactable))
        {
            interactable.ShowGuide(false);
            Interactable = null;
        }
    }

    public void EnterBuilding(GameObject roof)
    {
        if (roof == null) return;
        this.roof = roof;
        roof.SetActive(false);
    }

    public void ExitBuilding()
    {
        if(roof == null) return;
        roof.SetActive(true);
        roof = null;
    }

    public void ChangeFloor(Define.Layer floor)
    {
        Tr.position = new Vector3(Tr.position.x, Tr.position.y, GetZPosition(floor));
        SetViewByFloorChange(gameObject.layer, (int)floor);
        SetLayerRecursive(gameObject, (int)floor);
    }

    private void SetLayerRecursive(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            child.gameObject.layer = layer;

            Transform grandChild = child.GetComponentInChildren<Transform>();
            if (grandChild != null) { SetLayerRecursive(grandChild.gameObject, layer); }

        }
    }

    private int GetZPosition(Define.Layer floor)
    {
        if (floor == Define.Layer.Ground) return 0;
        else return ((int)Define.Layer.Floor1 - (int)floor) * 10;
    }

    private void SetViewByFloorChange(int prevFloor, int nextFloor)
    {
        if (prevFloor != (int)Define.Layer.Ground) Camera.main.cullingMask &= ~(1 << prevFloor);
        Camera.main.cullingMask |= 1 << nextFloor;
    }

    public Vector2 GetVelocity()
    {
        return MoveBody.Velocity;
    }
}
