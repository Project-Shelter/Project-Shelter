using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRangeWeapon : IWeapon
{
    int MaxAmmo { get; }
    int CurrentAmmo { get; }
    Action<int, int> OnAmmoChanged { get; set; }
    float ReloadDelay { get; }
    void Reload();
}
