using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_InteractableGuide : UI_Section
{
    enum Images
    {
        GuideImage,
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Image>(typeof(Images));
        Get<Image>((int)Images.GuideImage).gameObject.SetActive(false);
    }

    public void ShowGuide(bool onOff)
    {
        Get<Image>((int)Images.GuideImage).gameObject.SetActive(onOff);
    }
}
