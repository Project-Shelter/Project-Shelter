using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRangeWeapon : IWeapon
{
    bool CanReload { get; }
    bool HasToBeReload { get; }
    float ReloadDelay { get; }
    void Reload();
}
