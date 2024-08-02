using ItemContainer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void Init();
    void Active(Actor owner, ItemEffect weaponInfo);
    void SetVisibility(bool value);
    bool IsVisible { get; }
    Action OnAttack { get; set; }
    float AttackDelay { get; }
    void Attack();
}
