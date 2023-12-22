using DG.Tweening;
using FMOD.Studio;
using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Pool;

public class SentryGunController : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject bulletHolePrefab;
    [SerializeField] Transform muzzle;

    [SerializeField] bool scaleBulletSize = false;
    [ShowIf("scaleBulletSize")]
    [DetailedInfoBox("If you want to scale the bullet knockback based on the gun's scale, add an IScalable reference. ",
        "Null = force multiplier of 1.\n" +
        "Only need IScalable but cannot serialize interface. "
        )]
    [SerializeField] MonoBehaviour iScalable;


    [Title("Aim Properties")]
    [SerializeField] bool aimPlayer = false;
    [ShowIf("aimPlayer")]
    [SerializeField]
    bool lineOfSightRequired = false;
    [ShowIf("aimPlayer")]
    [SerializeField]
    bool fireIfNoLineOfSight = false;
    [ShowIf("aimPlayer")]
    [SerializeField]
    LayerMask playerLayer;
    [ShowIf("aimPlayer")]
    [SerializeField]
    [ReadOnly]
    Transform target;
    [ShowIf("aimPlayer")]
    [SerializeField] float aimSpeed = Mathf.PI;
    [ShowIf("aimPlayer")]
    [SerializeField] float aimDegree = 180f;

    [Title("Bullet Properties")]
    [SerializeField] float damage = 0f;
    [SerializeField] float fireRatePerMinute = 60;
    [SerializeField] float bulletRange = 100f;
    [SerializeField] float bulletSpeed = 100f;
    [SerializeField] float knockbackStrength = 0f;
    [SerializeField] int bulletReflectCount = 0;

    [SerializeField] FloatReference playerScale;
    [SerializeField] string fireSoundPath;
    float fireTimestamp;

    [Title("Debug")]
    [SerializeField] bool debug = false;
    [ShowIf("debug")]
    [SerializeField] bool dontShoot = false;

    IObjectPool<BaseBullet> bulletPool;
    IObjectPool<BaseBulletHole> bulletHolePool;

    Vector3 defaultFacing;

    float dotTarget = 0;


    IScalable scab;

    private void Start() {
        defaultFacing = transform.forward;
        bulletPool = new ObjectPool<BaseBullet>(BulletCreate, BulletOnTakeFromPool, BulletOnReleaseToPool, BulletOnDestroyPoolObject, true, 10, 60);
        bulletHolePool = new ObjectPool<BaseBulletHole>(BulletHoleCreate, OnTakeFromPool, OnReleaseToPool, OnDestroyPoolObject, true, 10, 60);

        if (aimPlayer) {
            DOTween.Sequence()
                .AppendCallback(() => PollForPlayer())
                .AppendInterval(1.0f)
                .SetLoops(-1);
        }

        dotTarget = -(((aimDegree) / 360) * 2 - 1f);
        if (iScalable != null) {
            scab = iScalable as IScalable;
        }
    }

    bool WithinLineOfSight(RaycastHit rh) {
        if (!lineOfSightRequired) { return true; }
        if (rh.transform != target) return false;
        return true;
    }

    void Update()
    {
        fireTimestamp += Time.deltaTime;

        bool _withinLineOfSight;
        if (target != null) {
            RaycastHit hit;
            Physics.Raycast(muzzle.position, (target.position + new Vector3(0, playerScale.Value / 2, 0)) - muzzle.position, out hit, bulletRange, playerLayer);

            _withinLineOfSight = WithinLineOfSight(hit);
        } else { 
            _withinLineOfSight = false;
        }

       
        Vector3 targetDirection = defaultFacing;
        if (aimPlayer && target != null && _withinLineOfSight) {
            Vector3 toTarget = (target.position - transform.position + new Vector3(0, playerScale.Value / 2, 0)).normalized;
            if (Vector3.Dot(toTarget, defaultFacing) > dotTarget) { targetDirection = toTarget; }
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection), aimSpeed * Time.deltaTime);


        // if facing where it needs to aim
        if (!Mathf.Approximately(Vector3.Dot(transform.forward, targetDirection), 1)) { return; }

        // shoot/dont shoot if player not in line of sight
        if (aimPlayer && !_withinLineOfSight && !fireIfNoLineOfSight) { return; }

        if (dontShoot) return;

        if (fireTimestamp > 60 / fireRatePerMinute) {
            fireTimestamp = 0;
            Fire();
        }
        
    }

    void PollForPlayer() {
        if (this.target != null) return;
        GameObject target = GameObject.FindWithTag("Player");
        if (target != null) {
            this.target = target.transform;
        }

    }
    void Fire() {
        BaseBullet b = bulletPool.Get();
        b.Initialize(damage, muzzle.position, muzzle.forward, bulletRange, bulletSpeed, knockbackStrength, bulletReflectCount, scab == null ? 1 : scab.GetCurrentScale());
        b.Launch();
        FireSound();
    }


    void FireSound() {
        if (fireSoundPath == "") return;
        EventInstance e = FMODUnity.RuntimeManager.CreateInstance(fireSoundPath);
        e.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        e.start();
        e.release();
    }

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
}
