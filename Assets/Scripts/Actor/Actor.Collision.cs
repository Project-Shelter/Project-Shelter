using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Actor : MonoBehaviour, ILivingEntity, IMovable
{
    #region CollisionVariables

    public IInteractable interactable;
    public BreakableObject concealment;
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
        if (other.gameObject.layer == (int)Define.Layer.EnemyTrigger)
        {
            countCollidingEnemy++;
            Buff.AddBuff(collidingEnemyDebuff);
        }
        if(other.TryGetComponent(out IInteractable interactable))
        {
            this.interactable = interactable;
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
            this.interactable = null;
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
        gameObject.layer = (int)floor;
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
