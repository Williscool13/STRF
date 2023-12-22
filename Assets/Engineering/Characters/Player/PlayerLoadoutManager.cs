using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadoutManager : MonoBehaviour
{
    WeaponSlot activeSlot = WeaponSlot.Primary;
    [SerializeField] Transform activeWeaponPivot;
    [SerializeField] Transform reserveWeaponPivot;
    [SceneObjectsOnly][SerializeField] Transform primaryWeapon;
    [SceneObjectsOnly][SerializeField] Transform secondaryWeapon;
    //[SerializeField] GunRigController gunRigController;

    Dictionary<int, WeaponBase> cachedWeapons = new();
    Dictionary<int, WeaponIkTarget> cachedWeaponIkTargets = new();

    public event EventHandler<WeaponBase> OnWeaponChange;
    private void Start() {
        DOTween.Sequence()
            .AppendInterval(0.1f)
            .AppendCallback(() => SwapWeaponParent(secondaryWeapon, primaryWeapon));
        //gunRigController.ChangeHandIKTargets(GetCachedWeaponIkTargets(primaryWeapon).Front, GetCachedWeaponIkTargets(primaryWeapon).Handle);
    }

    public void SwapWeaponTransforms() {
        Debug.Log("Swapping Weapons");
        WeaponSlot targetSlot = activeSlot == WeaponSlot.Primary ? WeaponSlot.Secondary : WeaponSlot.Primary;

        switch (targetSlot) {
            case WeaponSlot.Primary:
                SwapWeaponParent(secondaryWeapon, primaryWeapon);
                break;
            case WeaponSlot.Secondary:
                SwapWeaponParent(primaryWeapon, secondaryWeapon);
                break;
        }

        activeSlot = targetSlot;
    }


    void SwapWeaponParent(Transform current, Transform target) {
        SetWeaponParent(current, reserveWeaponPivot);
        GetCachedWeaponBaseReference(current.gameObject).OnWeaponUnequip();
        SetWeaponParent(target, activeWeaponPivot);
        GetCachedWeaponBaseReference(target.gameObject).OnWeaponEquip() ;

        OnWeaponChange?.Invoke(this, GetCachedWeaponBaseReference(target.gameObject));
        //gunRigController.ChangeHandIKTargets(GetCachedWeaponIkTargets(target).Front, GetCachedWeaponIkTargets(target).Handle);
    }

    void SetWeaponParent(Transform weapon, Transform pivot) {
        Vector3 localPos = weapon.localPosition;
        Quaternion localRot = weapon.localRotation;
        weapon.SetParent(pivot);
        weapon.SetLocalPositionAndRotation(localPos, localRot);
    }

    public WeaponBase GetCurrentWeapon() {
        switch (activeSlot) {
            case WeaponSlot.Primary:
                return GetCachedWeaponBaseReference(primaryWeapon.gameObject);
            case WeaponSlot.Secondary:
                return GetCachedWeaponBaseReference(secondaryWeapon.gameObject);
            default:
                break;
        }
        return null;
    }

    WeaponIkTarget GetCachedWeaponIkTargets(Transform weaponObject) {
        int instanceId = weaponObject.GetInstanceID();
        if (!cachedWeaponIkTargets.ContainsKey(instanceId)) {
            cachedWeaponIkTargets[instanceId] = new WeaponIkTarget(weaponObject.GetChild(0), weaponObject.GetChild(1));
        }
        return cachedWeaponIkTargets[instanceId];
    }
    WeaponBase GetCachedWeaponBaseReference(GameObject weaponObject) {
        int instanceId = weaponObject.GetInstanceID();
        if (!cachedWeapons.ContainsKey(instanceId)) {
            cachedWeapons[instanceId] = weaponObject.GetComponent<WeaponBase>();
        }
        return cachedWeapons[instanceId];
    }


    public void ReloadMovement(float reloadTime) {
        float localY = activeWeaponPivot.localPosition.y;

        WeaponBase targetWep = GetCurrentWeapon();

        targetWep.OnWeaponUnequip();
        Sequence seq = DOTween.Sequence()
            .Append(activeWeaponPivot.DOLocalMoveY(localY - 0.6f, reloadTime * 0.2f))
            .AppendInterval(reloadTime * 0.5f)
            .Append(activeWeaponPivot.DOLocalMoveY(localY, reloadTime * 0.3f));

        seq.OnComplete(() => { targetWep.OnWeaponEquip(); });
    }

    public void SwapMovement(float swapTime) {
        float localY = activeWeaponPivot.localPosition.y;
        WeaponBase targetWep = GetCurrentWeapon();
        targetWep.OnWeaponUnequip();
        Sequence seq = DOTween.Sequence()
            .Append(activeWeaponPivot.DOLocalMoveY(localY - 0.6f, swapTime * 0.4f))
            .AppendInterval(swapTime * 0.4f)
            .Append(activeWeaponPivot.DOLocalMoveY(localY, swapTime * 0.2f));
    }




    enum WeaponSlot
    {
        Primary,
        Secondary,
    }
    struct WeaponIkTarget
    {
        public Transform Front { get; private set; }
        public Transform Handle { get; private set; }

        public WeaponIkTarget(Transform front, Transform handle) {
            Front = front;
            Handle = handle;
        }
    }
}



