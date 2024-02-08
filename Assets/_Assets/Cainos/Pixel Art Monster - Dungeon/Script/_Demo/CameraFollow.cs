using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtMonster_Dungeon
{
    //let camera follow target
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float lerpSpeed = 1.0f;

        private bool initialized = false;
        private Vector3 offset;
        private Vector3 targetPos;


        private void Update()
        {
            if (target == null) return;

            if (initialized == false)
            {
                offset = transform.position - target.position;
                initialized = true;
            }

            targetPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
        }

    }
}
