/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Actor의 행동을 정하고 실행여부 결정(Input에 전달) : 명령패턴
public abstract class ActorCommand
{
    public abstract void Execute(in Actor actor);
}

public class IdleCommand : ActorCommand
{
    public override void Execute(in Actor actor)
    {
        actor.SetState(ActorState.Idle);
    }
}

public class RunRightCommand : ActorCommand
{
    public override void Execute(in Actor actor)
    {
        actor.GoRight = true;
        if (actor.IsGround) { actor.SetState(ActorState.Run); }
        else { actor.SetStateWithoutEnter(ActorState.Run); }
    }
}

public class RunLeftCommand : ActorCommand
{
    public override void Execute(in Actor actor)
    {       
        actor.GoRight = false;
        if (actor.IsGround) { actor.SetState(ActorState.Run); }
        else { actor.SetStateWithoutEnter(ActorState.Run); }
    }
}

public class JumpOrGoUpCommand : ActorCommand
{
    public override void Execute(in Actor actor)
    {
        if (actor.CanGoUpDown) { actor.SetState(ActorState.GoUp); }
        else { actor.SetState(ActorState.Jump);}
    }
}

public class DownOrGoDownCommand : ActorCommand
{
    public override void Execute(in Actor actor)
    {
        if (actor.CanGoUpDown) { actor.SetState(ActorState.GoDown); }
        else { actor.SetState(ActorState.Down);}
    }
}

public class AttackCommand : ActorCommand
{
    public override void Execute(in Actor actor)
    {
        actor.SetState(ActorState.Attack);
    }
}*/