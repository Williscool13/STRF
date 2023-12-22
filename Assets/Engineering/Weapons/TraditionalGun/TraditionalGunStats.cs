using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TraditionalGunStats", menuName = "ScriptableObjects/DataContainers/GunStats/TraditionalGunStats")]
public class TraditionalGunStats : ScriptableObject
{
    public float damage;
    public float range;
    public float speed;
    public float fireRate;
    public float reloadSpeedMultiplier;
    public int magazineSize;
    public int reserveAmmo;
    public bool infiniteAmmo;

    public bool isAutomatic;

    public Vector2 recoilPower;
    public float recoilDuration;
    public float kickbackStrength;
    public Vector2 cinemachineRecoilVelocity;

    public float knockbackStrength;

    
    public int reflectCount;
}
