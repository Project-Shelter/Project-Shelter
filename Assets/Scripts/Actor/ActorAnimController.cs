using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActorAnimParameter
{
    IsMoving,
    IsAttacking,
    IsDead,
    LookDirection,
    MoveDirection,
}
public class ActorAnimController
{
    private Animator anim;
    private Dictionary<ActorAnimParameter, int> AnimParameters = new();

    public ActorAnimController(Actor actor)
    {
        anim = Util.GetOrAddComponent<Animator>(actor.gameObject);
        InitAnimController();
        actor.MoveBody.OnLookDirChanged += SetLookDirection;
        actor.MoveBody.OnMoveDirChanged += SetMoveDirection;
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
        AnimParameters.Add(ActorAnimParameter.LookDirection, Animator.StringToHash("LookDirection"));
        AnimParameters.Add(ActorAnimParameter.MoveDirection, Animator.StringToHash("MoveDirection"));
    }

    #region Bind Functions

    private void SetLookDirection(Direction dir)
    {
        SetAnimParamter(ActorAnimParameter.LookDirection, (float)dir);
    }

    private void SetMoveDirection(Direction dir)
    {
        SetAnimParamter(ActorAnimParameter.MoveDirection, (float)dir);
    }

    #endregion

    public void SetAnimParamter(ActorAnimParameter animParamter, bool value)
    {
        anim.SetBool(AnimParameters[animParamter], value);
    }

    public void SetAnimParamter(ActorAnimParameter animParamter, int value)
    {
        anim.SetInteger(AnimParameters[animParamter], value);
    }

    public void SetAnimParamter(ActorAnimParameter animParamter, float value)
    {
        anim.SetFloat(AnimParameters[animParamter], value);
    }
    
    public void SetAnimParamter(ActorAnimParameter animParamter)
    {
        anim.SetTrigger(AnimParameters[animParamter]);
    }
}
