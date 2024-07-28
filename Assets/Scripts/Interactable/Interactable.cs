using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour, IInteractable
{
    protected Actor actor;
    private UI_InteractableGuide guide;


    protected virtual void Awake()
    {
        guide = Util.FindChild<UI_InteractableGuide>(gameObject, "UI_InteractableGuide", true);
    }

    public virtual void StartInteract(Actor actor)
    {
        this.actor = actor;
    }

    public abstract void Interacting();

    public abstract void StopInteract();

    public abstract bool CanKeepInteracting();

    public void ShowGuide(bool onOff)
    {
        if (guide != null) { guide.ShowGuide(onOff); }
    }
}
