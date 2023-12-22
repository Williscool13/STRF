using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{

    public abstract int GetAmmo { get; }
    public abstract int GetReserveAmmo { get; }
    public abstract float ReloadSpeedMultiplier { get; }
    public abstract bool IsInfinite { get; }
    public abstract bool CanFire(bool buttonHold);
    public abstract RecoilData Fire();

    public abstract bool CanReload();
    public abstract void ReloadStart();
    public abstract void ReloadEnd();

    public abstract void AimStart();
    public abstract void AimEnd();

    public event EventHandler OnWeaponEquipped;
    public event EventHandler OnWeaponUnequipped;
    public event EventHandler OnWeaponFired;
    public event EventHandler OnWeaponFinishReloaded;
    public event EventHandler OnWeaponDropped;
    public event EventHandler OnweaponPickedUp;

    public void OnWeaponEquip() {
        OnWeaponEquipped?.Invoke(this, null);
    }
    public void OnWeaponUnequip() {
        OnWeaponUnequipped?.Invoke(this, null);
    }

}

