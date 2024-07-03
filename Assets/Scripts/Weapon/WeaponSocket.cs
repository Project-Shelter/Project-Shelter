using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSocket : MonoBehaviour
{
    public IWeapon Weapon { get; private set; } // 추후 무기 변경 방식에 따라 바뀔 수 있음

    private void Awake()
    {
        Weapon = new RangeWeapon(transform, 10, 0.5f, 10f, 1f, 10f, 10, 10);
    }
}
