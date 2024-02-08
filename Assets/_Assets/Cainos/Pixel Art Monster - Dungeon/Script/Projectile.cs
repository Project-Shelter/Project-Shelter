using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtMonster_Dungeon
{
    public class Projectile : MonoBehaviour
    {
        public float lifeTime = 2.0f;
        public GameObject explosionPrefab;

        private float lifeTimer;

        private void Update()
        {
            lifeTimer += Time.deltaTime;
            if ( lifeTimer > lifeTime)
            {
                if (explosionPrefab) Explode();

                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (explosionPrefab)
            {
                Explode();
                Destroy(gameObject);
            }
        }

        private void Explode()
        {
            var explosion = Instantiate(explosionPrefab);
            explosion.transform.position = transform.position;
        }
    }
}
