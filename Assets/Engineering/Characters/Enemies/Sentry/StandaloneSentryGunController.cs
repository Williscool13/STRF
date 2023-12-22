using Combat;
using DG.Tweening;
using FMOD.Studio;
using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Pool;

public class StandaloneSentryGunController : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject bulletHolePrefab;
    [SerializeField] Transform muzzle;
    [SerializeField] EnemyHealthSystem healthSystem;


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
    [SerializeField] bool revertToDefaultFacing = false;
    [ShowIf("aimPlayer")]
    [SerializeField]
    bool clearShotRequired = false;
    [ShowIf("aimPlayer")]
    [SerializeField]
    bool fireIfNoClearshot = false;
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
    [ShowIf("aimPlayer")]
    [SerializeField] float aimTolerance = 1f;

    [Title("Bullet Properties")]
    [SerializeField] float damage = 0f;
    [SerializeField] float fireRatePerMinute = 60;
    [SerializeField] float bulletRange = 100f;
    [SerializeField] float bulletSpeed = 100f;
    [SerializeField] float knockbackStrength = 0f;
    [SerializeField] int bulletReflectCount = 0;

    [SerializeField] FloatReference playerScale;
    [SerializeField] string fireSoundPath;

    [Title("Debug")]
    [SerializeField] bool debug = false;
    [ShowIf("debug")]
    [SerializeField] bool dontShoot = false;

    float fireTimestamp;


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

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        col = GetComponent<Collider>();

        healthSystem.OnEnemyDeath += DeathThrow;
    }
    Rigidbody rb;
    Collider col;
    void DeathThrow(object o, HitDataContainer hitd) {
        rb.isKinematic = false;
        Bounds bounds = col.bounds;
        Vector3 forcePosition = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);

        rb.AddForceAtPosition(Vector3.up * 6.0f, forcePosition, ForceMode.Impulse);
        rb.AddForceAtPosition(Vector3.forward * 2.5f, forcePosition, ForceMode.Impulse);
        rb.AddForceAtPosition(-Vector3.right * 2.5f, forcePosition, ForceMode.Impulse);


        lowerChassis.GetComponent<Collider>().enabled = false;

        DOTween.Sequence()
            .AppendInterval(2.5f)
            .AppendCallback(() => Destroy(this.gameObject, 0.5f));

        this.enabled = false;
    }


    bool WithinLineOfSight(RaycastHit rh) {
        if (!clearShotRequired) { return true; }
        if (rh.transform != target) return false;
        return true;
    }

    void Update() {
        fireTimestamp += Time.deltaTime;

        bool _withinLineOfSight;
        if (target != null) {
            RaycastHit hit;
            Physics.Raycast(muzzle.position, (target.position + new Vector3(0, playerScale.Value / 2, 0)) - muzzle.position, out hit, bulletRange, playerLayer);

            _withinLineOfSight = WithinLineOfSight(hit);
        }
        else {
            _withinLineOfSight = false;
        }

        Vector3 targetDirection = defaultFacing;


        bool hasClearShot = false;
        if (target != null) {
            Vector3 toTarget = (target.position - transform.position + new Vector3(0, playerScale.Value / 2, 0)).normalized;
            bool _withinRotateDegree = Vector3.Dot(toTarget, defaultFacing) > dotTarget;

            hasClearShot = _withinLineOfSight && _withinRotateDegree;

            if (hasClearShot || !revertToDefaultFacing) {
                if (_withinRotateDegree) {
                    targetDirection = toTarget;

                }
            }
        }





        RotateStandaloneSentry(targetDirection);


        // if facing where it needs to aim
        if (!IsFacingYAxisDirection(targetDirection, aimTolerance)) { return; }
        //if (!Mathf.Approximately(Vector3.Dot(transform.forward, targetDirection), 1)) { return; }

        // shoot/dont shoot if player not in line of sight
        if (aimPlayer && clearShotRequired && !fireIfNoClearshot) {
            if (!hasClearShot) { return; }
        }


        if (dontShoot) return;
        if (fireTimestamp > 60 / fireRatePerMinute) {
            fireTimestamp = 0;
            Fire();
        }

    }

    [Title("Standalone Sentry Pivots")]
    [SerializeField] Transform chassisTransform;
    [SerializeField] Transform sentryTransform;
    [SerializeField] Transform lowerChassis;

    void RotateStandaloneSentry(Vector3 targetDir) {
        float horizontalRotation = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
        sentryTransform.rotation = Quaternion.RotateTowards(sentryTransform.rotation, Quaternion.Euler(0f, horizontalRotation, 0f), aimSpeed * Time.deltaTime);

        float verticalRotation = -Mathf.Atan2(targetDir.y, Mathf.Sqrt(targetDir.x * targetDir.x + targetDir.z * targetDir.z)) * Mathf.Rad2Deg;
        chassisTransform.localRotation = Quaternion.RotateTowards(chassisTransform.localRotation, Quaternion.Euler(0f, 0f, verticalRotation), aimSpeed * 3 * Time.deltaTime);

    }

    public bool IsFacingXAxisDirection(Vector3 targetDirection, float tolerance = 1f) {
        // Get the X-axis rotation of the chassis
        float currentXRotation = chassisTransform.localRotation.eulerAngles.x;

        // Calculate the target X-axis rotation
        float targetXRotation = -Mathf.Atan2(targetDirection.y, Mathf.Sqrt(targetDirection.x * targetDirection.x + targetDirection.z * targetDirection.z)) * Mathf.Rad2Deg;

        // Compare with the tolerance
        bool answer = Mathf.Abs(currentXRotation - targetXRotation) <= tolerance;
        return answer;
    }
    public bool IsFacingYAxisDirection(Vector3 targetDirection, float tolerance = 1f) {
        // Get the Y-axis rotation of the sentry
        float currentYRotation = sentryTransform.rotation.eulerAngles.y;

        // Calculate the target Y-axis rotation
        float targetYRotation = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;

        // Normalize the angles to be in the range of -180 to 180 degrees
        currentYRotation = Mathf.Repeat(currentYRotation + 180f, 360f) - 180f;
        targetYRotation = Mathf.Repeat(targetYRotation + 180f, 360f) - 180f;

        // Compare with the tolerance
        return Mathf.Abs(currentYRotation - targetYRotation) <= tolerance;
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
