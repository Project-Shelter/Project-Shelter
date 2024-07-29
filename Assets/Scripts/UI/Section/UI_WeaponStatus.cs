using ItemContainer;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponStatus : UI_Section
{
    enum Images
    {
        Background,
        WeaponImage,
    }
    enum Texts
    {
        AmmoText,
    }

    private Image background;
    private Image weaponImage;
    private TextMeshProUGUI ammoText;

    private ActorController actorController;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        background = GetImage((int)Images.Background);
        weaponImage = GetImage((int)Images.WeaponImage);
        ammoText = GetText((int)Texts.AmmoText);

        actorController = ServiceLocator.GetService<ActorController>();
        actorController.CurrentActor.OnItemChanged += UpdateWeaponStatus;
        actorController.BeforeSwitchActorAction -= () => 
            { actorController.CurrentActor.OnItemChanged -= UpdateWeaponStatus; };
        actorController.SwitchActorAction += () =>
            { actorController.CurrentActor.OnItemChanged += UpdateWeaponStatus; };
    }

    private void UpdateWeaponStatus(ItemVO item)
    {
        if (item == null || item.id == 0)
        {
            UpdateNullWeapon();
            return;
        }

        IWeapon weapon = actorController.CurrentActor.WeaponSocket.Weapon;
        ItemData itemData = ItemDummyData.ItemDB.data[item.id];
        string name = itemData.name;

        if (weapon == null)
        {
            UpdateNullWeapon();
        }
        else if (weapon is IMeleeWeapon)
        {
            UpdateMeleeWeapon(name);
        }
        else if(weapon is IRangeWeapon range)
        {
            range.OnAmmoChanged += UpdateRangeAmmo;
            UpdateRangeWeapon(name, range.MaxAmmo, range.CurrentAmmo);
        }
    }

    private void UpdateNullWeapon()
    {
        background.enabled = false;
        weaponImage.sprite = null;
        weaponImage.color = Color.clear;
        ammoText.text = "";
    }

    private void UpdateMeleeWeapon(string name)
    {
        background.enabled = true;
        weaponImage.sprite = Managers.Resources.Load<Sprite>($"Arts/Items/{name}");
        weaponImage.color = Color.white;
        ammoText.text = "∞";
    }

    private void UpdateRangeWeapon(string name, int maxAmmo, int currentAmmo)
    {
        background.enabled = true;
        weaponImage.sprite = Managers.Resources.Load<Sprite>($"Arts/Items/{name}");
        weaponImage.color = Color.white;
        UpdateRangeAmmo(maxAmmo, currentAmmo);
    }

    private void UpdateRangeAmmo(int maxAmmo, int currentAmmo)
    {
        ammoText.text = $"{currentAmmo}/{maxAmmo}";
        if (currentAmmo == 0) { ammoText.color = Color.red; }
        else { ammoText.color = Color.white; }
    }
}
