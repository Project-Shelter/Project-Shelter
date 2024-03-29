using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    private IFloor floor;

    private void Start()
    {
        floor = transform.parent.GetComponent<IFloor>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IMovable movable = collision.GetComponent<IMovable>();
        if(movable != null)
        {
            floor.PassExit(movable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IMovable movable = collision.GetComponent<IMovable>();
        if (movable != null)
        {
            floor.PassExit(movable);
        }
    }
}
