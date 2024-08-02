using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concealment : Interactable
{
    private BreakableObject parentObject;
    private Collider2D coll;

    protected override void Awake()
    {
        base.Awake();
        parentObject = GetComponentInParent<BreakableObject>();
        coll = GetComponent<Collider2D>();
    }

    private void Start()
    {
        parentObject.AddOnDeath(StopInteract);
    }

    public override void StartInteract(Actor actor)
    {
        base.StartInteract(actor);
        actor.Anim.SetAnimParamter(ActorAnimParameter.IsConcealing, true);
        actor.Tr.position = coll.bounds.center;
        actor.Concealment = parentObject;
    }

    public override void StopInteract()
    {
        if(actor == null) { return; }
        actor.Anim.SetAnimParamter(ActorAnimParameter.IsConcealing, false);
        actor.Concealment = null;
    }

    public override void Interacting()
    {
        actor.Aim();
    }

    public override bool CanKeepInteracting()
    {
        if (InputHandler.ButtonEDown)
        {
            return false;
        }
        return true;
    }
}
