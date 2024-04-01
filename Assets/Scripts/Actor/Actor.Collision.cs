using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Actor : MonoBehaviour, ILivingEntity, IMovable
{
    #region CollisionVariables

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
    }

    public void EnterBuilding(GameObject roof, Define.Layer floor)
    {
        roof.SetActive(false);
        this.roof = roof;
        Camera.main.cullingMask |= 1 << (int)floor;
    }

    public void ExitBuilding(Define.Layer floor)
    {
        roof.SetActive(true);
        this.roof = null;
        Camera.main.cullingMask &= ~(1 << (int)floor);
    }

    public void ChangeFloor(Define.Layer floor)
    {
        int newZ;
        if(floor == Define.Layer.Ground)
        {
            newZ = 0;
        }
        else
        {
            newZ = (int)Define.Layer.Floor1 - (int)floor;
        }
        Tr.position = new Vector3(Tr.position.x, Tr.position.y, newZ);
        gameObject.layer = (int)floor;
    }
}
