using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LookDirection
{
    Up, Down, Left, Right
}

public class ActorMoveBody
{
    private Actor actor;
    private Rigidbody2D rigid;

    #region Movement States

    public bool CanMove { get { return ((HorizontalAxis != 0) || (VerticalAxis != 0)); } }
    public bool CanDash { get { return InputHandler.ButtonSpace && isPassedDashCool; } }
    public LookDirection LookDirection { get; private set; }

    #endregion

    #region Movement Variables
    
    private int HorizontalAxis { get { return (InputHandler.ButtonA ? -1 : 0) + (InputHandler.ButtonD ? 1 : 0); } }
    private int VerticalAxis { get { return (InputHandler.ButtonS ? -1 : 0) + (InputHandler.ButtonW ? 1 : 0); } }
    private bool isPassedDashCool = true;
    private readonly Quaternion left = Quaternion.Euler(new Vector3(0f, 180f, 0f));
    private readonly Quaternion right = Quaternion.Euler(new Vector3(0f, 0f, 0f));

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

        if(CanMove) Move(speed);
        else rigid.velocity = actor.Tr.right * speed; // 추후 서있는것이 4방향으로 가능해지면 교체
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
        // TODO - 오른쪽 마우스 누르고 있으면 LookDirection 방향, 아니면 axis 방향
        if (HorizontalAxis == -1) actor.Tr.rotation = left;
        if (HorizontalAxis == 1) actor.Tr.rotation = right;


        //if (VerticalAxis == -1) actor.Anim.SetAnimParamter(ActorAnimParameter.LookDirection, (int)LookDirection);
    }
    public void Stop()
    {
        rigid.velocity = Vector2.zero;
    }
}
