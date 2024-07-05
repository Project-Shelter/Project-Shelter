using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class WeaponSocket : MonoBehaviour
{
    public IWeapon Weapon { get; private set; } // 추후 무기 변경 방식에 따라 바뀔 수 있음

    private void Awake()
    {
        Weapon = Instantiate(Managers.Resources.Load<RangeWeapon>("Prefabs/Weapon/RangeWeapon"), transform);
    }
}
