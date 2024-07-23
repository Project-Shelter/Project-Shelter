using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSocket : MonoBehaviour
{
    private IWeapon weapon;
    public IWeapon Weapon {
        get 
        {
            if (weapon.IsActived) { return weapon; }
            else { return null; } 
        }
        private set 
        {
            weapon = value;
        } 
    } 
    // 추후 무기 변경 방식에 따라 바뀔 수 있음
    private Actor owner;
    private bool isRange = false;

    private void Start()
    {
        owner = GetComponentInParent<Actor>();
        owner.MoveBody.OnLookDirChanged += SetRotationZero;
        UpdateWeapon();
    }

    private void Update()
    {
        if(owner.IsAiming && isRange)
        {
            RotateToMousePos();
        }
    }

    // 나중엔 인자로 받아서 무기 변경 가능하게
    public void UpdateWeapon()
    {
        weapon = Instantiate(Managers.Resources.Load<MeleeWeapon>("Prefabs/Weapon/MeleeWeapon"), transform);
        weapon.Init(owner);
        weapon.SetActive(true);

        if(weapon is IRangeWeapon)
        {
            isRange = true;
            UpdateRangeWeapon(weapon as IRangeWeapon);
        }
        else if(weapon is IMeleeWeapon)
        {
            isRange = false;
            UpdateMeleeWeapon(weapon as IMeleeWeapon);
        }
    }

    private void UpdateMeleeWeapon(IMeleeWeapon meleeWeapon)
    {

    }

    private void UpdateRangeWeapon(IRangeWeapon rangeWeapon)
    {
        rangeWeapon.OnAttack += RotateToMousePos;
    }

    public void SetWeaponActive(bool value)
    {
        if (weapon != null) { weapon.SetActive(value); }
    }

    public void RotateToMousePos()
    {
        Direction direction = owner.MoveBody.LookDir;
        float anglePreset = 0;
        switch (direction)
        {
            case Direction.Up:
                anglePreset = -90;
                break;
            case Direction.Down:
                anglePreset = 90;
                break;
            case Direction.Left:
                anglePreset = 180;
                break;
            case Direction.Right:
                anglePreset = 0;
                break;
        }

        Vector2 mousePos = InputHandler.MousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 dir = (worldPos - (Vector2)transform.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + anglePreset);
    }

    private void SetRotationZero(Direction dir)
    {
        print("SetRotationZero");
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
