using Cinemachine;
using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class TraditionalGun : WeaponBase
{
    [SerializeField] private TraditionalGunStats stats;

    [SerializeField] private GunSoundManager soundManager;

    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bulletHolePrefab;

    
    [SerializeField] private CinemachineImpulseSource recoilSource;
    [SerializeField] private CinemachineImpulseSource kickbackSource;

    [SerializeField] FloatReference playerScale;

    [SerializeField] private TextMeshPro ammoCountUI;
    int currentAmmo;
    int currentReserveAmmo;

    IObjectPool<BaseBullet> bulletPool;
    IObjectPool<BaseBulletHole> bulletHolePool;
    float timeSinceLastFire = 0.0f;

    public override int GetAmmo => currentAmmo;
    public override int GetReserveAmmo => currentReserveAmmo;
    public override float ReloadSpeedMultiplier => stats.reloadSpeedMultiplier;
    public override bool IsInfinite => stats.infiniteAmmo;

    void Start()
    {
        currentAmmo = stats.magazineSize;
        if (ammoCountUI != null) ammoCountUI.text = currentAmmo.ToString();
        currentReserveAmmo = stats.reserveAmmo;
        bulletPool = new ObjectPool<BaseBullet>(BulletCreate, BulletOnTakeFromPool, BulletOnReleaseToPool, BulletOnDestroyPoolObject, true, 10, 60);
        bulletHolePool = new ObjectPool<BaseBulletHole>(BulletHoleCreate, OnTakeFromPool, OnReleaseToPool, OnDestroyPoolObject, true, 10, 60);
    }


    private void Update() {
        timeSinceLastFire += Time.deltaTime;
    }

    public override bool CanFire(bool buttonHold) {
        return stats.isAutomatic ? AutomaticCheck() : SemiCheck(buttonHold);
    }

    bool AutomaticCheck() {
        if (timeSinceLastFire < 60 / stats.fireRate) { return false; }
        return currentAmmo > 0;
    }

    bool SemiCheck(bool buttonHold) {
        if (timeSinceLastFire < 60 / stats.fireRate) { return false; }
        if (buttonHold) { return false; }
        return currentAmmo > 0;
    }


    [SerializeField] private NullEvent shotFired;

    public override RecoilData Fire() {
        currentAmmo -= 1;
        if (ammoCountUI != null) ammoCountUI.text = currentAmmo.ToString();
        BaseBullet b = bulletPool.Get();
        b.Initialize(stats.damage, muzzleTransform.position, transform.forward, stats.range, stats.speed, stats.knockbackStrength, stats.reflectCount, playerScale.Value);
        b.transform.localScale = Vector3.one * playerScale.Value;
        b.Launch();
        soundManager.FireSound();

        timeSinceLastFire = 0.0f;

        recoilSource.GenerateImpulseWithVelocity(new Vector3(stats.cinemachineRecoilVelocity.x, stats.cinemachineRecoilVelocity.y, 0));
        kickbackSource.GenerateImpulseWithVelocity(new Vector3(0, 0, -stats.kickbackStrength));

        if (shotFired != null) shotFired.Raise(null);

        return new RecoilData(new Vector2(stats.recoilPower.x, stats.recoilPower.y), stats.recoilDuration);
    }

    
    public override bool CanReload() {
        return currentReserveAmmo > 0 && currentAmmo < stats.magazineSize;
    }

    public override void ReloadEnd() {
        if (currentAmmo + currentReserveAmmo <= stats.magazineSize) {
            currentAmmo += currentReserveAmmo;
            currentReserveAmmo = 0;
            return;
        }

        int diff = stats.magazineSize - currentAmmo;
        currentAmmo = stats.magazineSize; 
        if (ammoCountUI != null) ammoCountUI.text = currentAmmo.ToString();

        currentReserveAmmo -= diff;

        if (stats.infiniteAmmo) {
            currentReserveAmmo = stats.reserveAmmo;
        }
    }

    public override void ReloadStart() {
        soundManager.ReloadSound();
    }

    public override void AimStart() {
        soundManager.ADSEnterSound();
    }

    public override void AimEnd() {
        soundManager.ADSExitSound();
    }


    #region Bullet Pooling
    BaseBullet BulletCreate() {
        GameObject bullet = Instantiate(bulletPrefab, new Vector3(-100, -100, -100), Quaternion.identity);
        BaseBullet baseBullet = bullet.GetComponent<BaseBullet>();

        baseBullet.SetBulletPooler(bulletPool);
        baseBullet.SetBulletHolePooler(bulletHolePool);
        return baseBullet;
    }

    void BulletOnReleaseToPool(BaseBullet item) {
        item.gameObject.SetActive(false);
    }
    void BulletOnTakeFromPool(BaseBullet item) {
        item.gameObject.SetActive(true);
    }
    void BulletOnDestroyPoolObject(BaseBullet item) {
        Destroy(item.gameObject);
    }
    #endregion

    #region Bullet Hole Pooling
    BaseBulletHole BulletHoleCreate() {
        GameObject bulletHole = Instantiate(bulletHolePrefab, new Vector3(-100, -100, -100), Quaternion.identity);
        BaseBulletHole baseBulletHole = bulletHole.GetComponent<BaseBulletHole>();

        baseBulletHole.SetBulletHolePooler(bulletHolePool);

        return baseBulletHole;
    }

    void OnReleaseToPool(BaseBulletHole item) {
        item.gameObject.SetActive(false);
    }

    void OnTakeFromPool(BaseBulletHole item) {
        item.gameObject.SetActive(true);
    }

    void OnDestroyPoolObject(BaseBulletHole item) {
        Destroy(item.gameObject);
    }

    #endregion
}
