using System;
using System.Collections;
using UnityEngine;

public enum Direction
{
    Up, Down, Left, Right, None
}

public class ActorMoveBody
{
    private Actor actor;
    private Rigidbody2D rigid;

    #region Movement States

    public bool CanMove { get { return ((HorizontalAxis != 0) || (VerticalAxis != 0)); } }
    public bool CanDash { get { return InputHandler.ButtonSpace && isPassedDashCool; } }
    public Direction LookDir 
    {
        get { return lookDir; }
        private set
        {
            if(value != lookDir)
            {
                lookDir = value;
                OnLookDirChanged?.Invoke(value);
            }
        } 
    }
    public Direction MoveDir 
    { 
        get { return moveDir; } 
        private set 
        {
            if (value != moveDir)
            {
                moveDir = value;
                OnMoveDirChanged?.Invoke(value);
            }
        } 
    }

    #endregion

    #region Movement Variables
    
    public Vector2 Velocity { get { return rigid.velocity; } }
    private int HorizontalAxis { get { return (InputHandler.ButtonA ? -1 : 0) + (InputHandler.ButtonD ? 1 : 0); } }
    private int VerticalAxis { get { return (InputHandler.ButtonS ? -1 : 0) + (InputHandler.ButtonW ? 1 : 0); } }
    private Direction lookDir;
    private Direction moveDir;
    private bool isPassedDashCool = true;

    #endregion

    #region Movement Events

    public event Action<Direction> OnLookDirChanged;
    public event Action<Direction> OnMoveDirChanged;

    #endregion

    public ActorMoveBody(Actor actor)
    {
        this.actor = actor;
        rigid = Util.GetOrAddComponent<Rigidbody2D>(actor.gameObject);
    }

    private void Move(float speed)
    {
        Vector2 velocity = new Vector2(HorizontalAxis, VerticalAxis) * speed;
        velocity = velocity.normalized * speed;
        rigid.velocity = velocity;
    }
    public void Move()
    {
        Move(actor.Stat.moveSpeed.GetValue());
    }

    public void Dash()
    {
        isPassedDashCool = false;
        float speed = actor.Stat.dashSpeed.GetValue();

        if (CanMove) Move(speed);
        else 
        {
            if (LookDir == Direction.Up) rigid.velocity = Vector2.up * speed;
            else if (LookDir == Direction.Down) rigid.velocity = Vector2.down * speed;
            else if (LookDir == Direction.Left) rigid.velocity = Vector2.left * speed;
            else if (LookDir == Direction.Right) rigid.velocity = Vector2.right * speed;
        }
    }

    public void DashOnCool()
    {
        CoroutineHandler.StartStaticCoroutine(WaitForDashCool());
    }
    private IEnumerator WaitForDashCool()
    {
        yield return new WaitForSeconds(actor.Stat.dashCoolTime.GetValue());
        isPassedDashCool = true;
    }

    public void Turn()
    {
        LookDir = InputHandler.MouseSection;
        if(LookDir == Direction.None) LookDir = MoveDir;

        if (HorizontalAxis == 0 && VerticalAxis == 0) MoveDir = LookDir;
        else if (HorizontalAxis == 1) MoveDir = Direction.Right;
        else if (HorizontalAxis == -1) MoveDir = Direction.Left;
        else if (VerticalAxis == 1) MoveDir = Direction.Up;
        else if (VerticalAxis == -1) MoveDir = Direction.Down;
    }
    public void Stop()
    {
        rigid.velocity = Vector2.zero;
    }
}
