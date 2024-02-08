using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    GameObject player;

    public Sprite OpenLeft;
    public Sprite Closed;
    public Sprite OpenRight;

    public bool IsOpen;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < 1 && Input.GetButtonDown("Action Key"))
        {
            if (IsOpen == false)
            {
                OpenDoor();
            }
            else
            {
                CloseDoor();
            }
        }
    }

    void CloseDoor()
    {
        GetComponent<SpriteRenderer>().sprite = Closed;
        GetComponent<BoxCollider2D>().enabled = true;
        IsOpen = false;
    }

    void OpenDoor()
    {
        if (player.transform.position.x > transform.position.x)
        {
            GetComponent<SpriteRenderer>().sprite = OpenLeft;
            GetComponent<BoxCollider2D>().enabled = false;
            IsOpen = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = OpenRight;
            GetComponent<BoxCollider2D>().enabled = false;
            IsOpen = true;
        }
    }

}
