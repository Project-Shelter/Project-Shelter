using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleController : MonoBehaviour
{
    public GameObject RaycastStartPoint;
    public float DestroyDelay = 0.75f;
    private bool Ready = true;

    // Update is called once per frame
    void Update()
    {
        // Scan for target and fall
        if (Ready)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(RaycastStartPoint.transform.position, Vector2.down);
            if (hitInfo.collider.gameObject.CompareTag("Player"))
            {
                gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
                Ready = false;
            }
        }
    }

    // Play animation and destroy object
    void OnCollisionEnter2D(Collision2D other)
    {
        {
            GetComponent<Animator>().SetTrigger("Shatter");
        }
        Destroy(gameObject, DestroyDelay);
    }
}