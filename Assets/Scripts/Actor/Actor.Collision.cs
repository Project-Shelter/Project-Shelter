using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Actor : MonoBehaviour, ILivingEntity
{
    #region CollisionVariables

    private int countCollidingEnemy;
    private SlowDebuff collidingEnemyDebuff;

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
}
