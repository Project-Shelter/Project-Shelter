using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance
{
    private Direction enterDirection;
    private Direction exitDirection;

    private IPathway pathway;
    public Entrance(IPathway pathway, Direction enterDirection)
    {
        this.pathway = pathway;
        this.enterDirection = enterDirection;

        if(enterDirection == Direction.Up)
        {
            exitDirection = Direction.Down;
        }
        else if(enterDirection == Direction.Down)
        {
            exitDirection = Direction.Up;
        }
        else if(enterDirection == Direction.Left)
        {
            exitDirection = Direction.Right;
        }
        else if(enterDirection == Direction.Right)
        {
            exitDirection = Direction.Left;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        IMovable movable = collision.GetComponent<IMovable>();
        if (movable != null)
        {
            Vector2 velocity = movable.GetVelocity();
            if (enterDirection == Direction.Left && velocity.x < 0)
                pathway.PassEntrance(movable);
            if (enterDirection == Direction.Right && velocity.x > 0)
                pathway.PassEntrance(movable);
            if (enterDirection == Direction.Up && velocity.y > 0)
                pathway.PassEntrance(movable);
            if (enterDirection == Direction.Down && velocity.y < 0)
                pathway.PassEntrance(movable);

            if (exitDirection == Direction.Left && velocity.x <= 0)
                pathway.PassExit(movable);
            if (exitDirection == Direction.Right && velocity.x >= 0)
                pathway.PassExit(movable);
            if (exitDirection == Direction.Up && velocity.y >= 0)
                pathway.PassExit(movable);
            if (exitDirection == Direction.Down && velocity.y <= 0)
                pathway.PassExit(movable);

        }
    }
}
