using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Interactable : MonoBehaviour, IInteractable
{
    protected Actor actor;
    private Image guide;


    protected virtual void Awake()
    {
        guide = Util.FindChild<Image>(gameObject, "GuideImage", true);
        guide.gameObject.SetActive(false);
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
        if (guide != null) { guide.gameObject.SetActive(onOff); }
    }
}
