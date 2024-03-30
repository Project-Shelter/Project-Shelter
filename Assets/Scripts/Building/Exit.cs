using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    private IPathway pathway;

    private void Start()
    {
        pathway = transform.parent.GetComponent<IPathway>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IMovable movable = collision.GetComponent<IMovable>();
        if(movable != null)
        {
            pathway.PassExit(movable);
        }
    }
}
