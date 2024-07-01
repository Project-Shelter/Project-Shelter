using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static ActorHealth;
using System;

public class UI_AttackedBody : UI_Section
{
    enum Images
    {
        Head,
        Trunk,
        RightArm,
        LeftArm,
        RightLeg,
        LeftLeg
    }

    delegate void AttackedFunc();
    private Dictionary<AttackedPart, AttackedFunc> attackedDict = new();

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Image>(typeof(Images));

        InitDictionary();
        SetActor();
        
        ServiceLocator.GetService<ActorController>().BeforeSwitchActorAction += ResyncActor;
        ServiceLocator.GetService<ActorController>().SwitchActorAction += ReloadImages;
    }

    private void ReloadImages()
    {
        ResetImages();
        SetActor();
    }

    private void ResetImages()
    {
        foreach (var image in Enum.GetValues(typeof(Images)))
        {
            GetImage((int)image).color = Color.white;
        }
    }

    private void InitDictionary() 
    {
        attackedDict.Add(AttackedPart.Head, HeadAttacked);
        attackedDict.Add(AttackedPart.Trunk, TrunkAttacked);
        attackedDict.Add(AttackedPart.Arm, ArmAttacked);
        attackedDict.Add(AttackedPart.Leg, LegAttacked);
        attackedDict.Add(AttackedPart.Normal, null);
    }

    private void SetActor()
    {
        ServiceLocator.GetService<ActorController>().CurrentActor.AttackedAction += BodyAttacked;
    }

    private void ResyncActor()
    {
        ServiceLocator.GetService<ActorController>().CurrentActor.AttackedAction -= BodyAttacked;
    }

    private void BodyAttacked(AttackedPart attackedPart)
    {
        if (attackedDict[attackedPart] != null) { attackedDict[attackedPart](); }
    }

    private void HeadAttacked()
    {
        StartCoroutine(Attacked(Images.Head));
    }
    private void TrunkAttacked()
    {
        StartCoroutine(Attacked(Images.Trunk));
    }
    private void ArmAttacked()
    {
        StartCoroutine(Attacked(Images.RightArm));
        StartCoroutine(Attacked(Images.LeftArm));
    }
    private void LegAttacked()
    {
        StartCoroutine(Attacked(Images.RightLeg));
        StartCoroutine(Attacked(Images.LeftLeg));
    }
    IEnumerator Attacked(Images attackedPart)
    {
        GetImage((int)attackedPart).color = Color.red;
        yield return new WaitForSeconds(0.5f);
        GetImage((int)attackedPart).color = Color.white;
    }
}
