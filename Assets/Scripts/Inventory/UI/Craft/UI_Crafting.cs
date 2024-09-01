using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Crafting : UI_Section
{
    private const int countMaterials = 8;

    private UI_CraftItem item;
    private Image[] materials = new Image[countMaterials];
    private Button craftButton;

    enum Buttons
    {
        CraftButton,
    }

    public override void Init()
    {
        base.Init();
        BindInstances();
    }

    void Start()
    {
        Init();
    }

    //UI_CraftItem 업데이트, 제작 재료 업데이트
    public void UpdateCraftItem(CraftItem craftItem, Sprite[] materials)
    {
        item.UpdateView(craftItem);
        for (int i = 0; i < countMaterials; i++)
        {
            this.materials[i].sprite = materials[i];
        }
    }

    public void InteractCraftButton(bool isInteratable)
    {
        craftButton.interactable = isInteratable;
    }

    private void BindInstances()
    {
        string[] materialIcons = new string[countMaterials];
        for (int i = 0; i < countMaterials; i++)
        {
            materialIcons[i] = "CraftItemIcon_" + i;
        }
        
        Bind<Button>(typeof(Buttons));
        Bind<Image>(materialIcons);

        item = Util.FindChild<UI_CraftItem>(gameObject, "CraftItem", false);//transform.FindObjectOfType<UI_CraftItem>();

        craftButton = GetButton((int)Buttons.CraftButton);
        for (int i = 0; i < countMaterials; i++)
        {
            materials[i] = GetImage(i);
        }
    }
}
