using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSocket : MonoBehaviour
{
    public IWeapon Weapon { get; private set; } // 추후 무기 변경 방식에 따라 바뀔 수 있음
    private Actor owner;

    private void Start()
    {
        owner = GetComponentInParent<Actor>();
        owner.MoveBody.OnLookDirChanged += SetRotation;
        UpdateWeapon();
    }

    private void Update()
    {
        if(owner.IsAiming)
        {
            RotateToMousePos();
        }
    }

    // 나중엔 인자로 받아서 무기 변경 가능하게
    private void UpdateWeapon()
    {
        Weapon = Instantiate(Managers.Resources.Load<RangeWeapon>("Prefabs/Weapon/RangeWeapon"), transform);
        Weapon.OnAttack += RotateToMousePos;
        Weapon.Init(owner);
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

    private void SetRotation(Direction dir)
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
