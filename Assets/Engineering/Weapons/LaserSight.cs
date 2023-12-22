using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] WeaponBase weaponParent;
    [SerializeField] float laserSightLength = 100.0f;
    private void Start() {
        weaponParent.OnWeaponEquipped += OnWeaponEquip;
        weaponParent.OnWeaponUnequipped += OnWeaponUnequip;

        laserSightActive = true;
        lineRenderer.enabled = true;
    }


    void Update()
    {
        if (laserSightActive) {
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, out hit, laserSightLength);
            if (hit.collider != null) {
                lineRenderer.SetPosition(1, transform.InverseTransformPoint(hit.point));
            } else {
                lineRenderer.SetPosition(1, transform.InverseTransformPoint(transform.position + transform.forward * laserSightLength));
            }
        }
    }

    bool laserSightActive = false;
    public void OnWeaponEquip(object o, EventArgs e) {
        laserSightActive = true;
        lineRenderer.enabled = true;
    }

    public void OnWeaponUnequip(object o, EventArgs e) {
        laserSightActive = false;
        lineRenderer.enabled = false;
    }
}
