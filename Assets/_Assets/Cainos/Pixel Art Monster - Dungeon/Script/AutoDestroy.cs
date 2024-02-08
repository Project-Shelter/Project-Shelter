using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtMonster_Dungeon
{
    public class AutoDestroy : MonoBehaviour
    {
        public float lifeTime = 2.0f;

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }
    }
}
