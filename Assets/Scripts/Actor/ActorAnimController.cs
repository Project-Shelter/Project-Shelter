using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActorAnimParameter
{
    IsMoving,
    IsAttacking,
    IsDead,
}
public class ActorAnimController
{
    private Animator anim;
    private Dictionary<ActorAnimParameter, int> AnimParameters = new();

    public ActorAnimController(Actor actor)
    {
        anim = Util.GetOrAddComponent<Animator>(actor.gameObject);
        InitAnimController();
    }
    
    private void InitAnimController()
    {
        GetParameterId();
    }

    private void GetParameterId()
    {
        AnimParameters.Add(ActorAnimParameter.IsMoving, Animator.StringToHash("IsMoving"));
        AnimParameters.Add(ActorAnimParameter.IsAttacking, Animator.StringToHash("IsAttacking"));
        AnimParameters.Add(ActorAnimParameter.IsDead, Animator.StringToHash("IsDead"));
    }
    
    public void SetAnimParamter(ActorAnimParameter animParamter, bool value)
    {
        SetAnimParamter(AnimParameters[animParamter], value);
    }

    private void SetAnimParamter(int animHash, bool value)
    {
        anim.SetBool(animHash, value);
    }

    public void SetAnimParamter(ActorAnimParameter animParamter, float value)
    {
        SetAnimParamter(AnimParameters[animParamter], value);
    }
    private void SetAnimParamter(int animHash, float value)
    {
        anim.SetFloat(animHash, value);
    }

    public void SetAnimParamter(ActorAnimParameter animParamter, int value)
    {
        SetAnimParamter(AnimParameters[animParamter], value);
    }
    private void SetAnimParamter(int animHash, int value)
    {
        anim.SetInteger(animHash, value);
    }

    public void SetAnimParamter(ActorAnimParameter animParamter)
    {
        SetAnimParamter(AnimParameters[animParamter]);
    }
    private void SetAnimParamter(int animHash)
    {
        anim.SetTrigger(animHash);
    }
}
