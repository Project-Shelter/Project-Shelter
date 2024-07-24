using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concealment : MonoBehaviour, IInteractable
{
    private BreakableObject parentObject;
    private Collider2D coll;
    private Actor actor;

    private void Awake()
    {
        parentObject = GetComponentInParent<BreakableObject>();
        coll = GetComponent<Collider2D>();
    }

    private void Start()
    {
        parentObject.AddOnDeath(StopInteract);
    }

    public void Interact(Actor actor)
    {
        this.actor = actor;
        actor.Anim.SetAnimParamter(ActorAnimParameter.IsConcealing, true);
        actor.Tr.position = coll.bounds.center;
        actor.Concealment = parentObject;
    }

    public void StopInteract()
    {
        if(actor == null) { return; }
        actor.Anim.SetAnimParamter(ActorAnimParameter.IsConcealing, false);
        actor.Concealment = null;
    }
}
