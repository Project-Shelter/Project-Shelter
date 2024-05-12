using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAnimController
{
    public float IdleMove;
    private Animator anim;
    private Dictionary<ActorState, string> StateAnim = new Dictionary<ActorState, string>();

    public ActorAnimController(Animator anim)
    {
        this.anim = anim;
        //InitAnimController();
    }
    
    //세부조정이 불가능할 것 같아서 일단은 폐기 -> Blend 적용하지 않을 생각이면 그냥 해도 괜찮을듯
    /*
    private void InitAnimController()
    {
        SetStateAnim();
    }

    private void SetStateAnim()
    {
        StateAnim.Add(ActorState.Idle, "IDLE");
        StateAnim.Add(ActorState.Move, "MOVE");
        StateAnim.Add(ActorState.OnLadder, "ONAIR");
    }*/
    
    public void Play(ActorState actorState, float value)
    {
        Play(StateAnim[actorState], value);
    }

    public void Play(string animName, float value)
    {
        Play(animName, value, 10.0f);
    }

    public void Play(string animName, float value, float blendTime)
    {
        IdleMove = Mathf.Lerp(IdleMove, value, blendTime * Time.deltaTime);
        anim.SetFloat("Idle_Move", IdleMove);
        anim.Play(animName);
    }
}
