using Combat;
using DG.Tweening;
using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.VFX;

public class TraditionalGunBullet : BaseBullet
{
    [SerializeField] LayerMask targetMasks;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] MeshRenderer bulletRenderer;

    [SerializeField] ParticleSystem sparks;


    int maxReflects = 0;

    [SerializeField] BulletSoundManager bulletSoundManager;
    Vector3 direction;
    float damage;
    float range;
    float speed;
    float knockbackStrength;

    bool launched;
    bool finished;
    float distanceTravelled;

    float shooterScaleAtTimeOfLaunch;

    int reflects = 0;
    private void Start() {
        sparks.transform.SetParent(null);
        sparks.Stop();
        sparks.transform.hideFlags = HideFlags.HideInHierarchy;
        this.transform.hideFlags = HideFlags.HideInHierarchy;
    }

    private void Update() {
        if (!launched) return;
        if (finished) return;

        Travel();
    }

    void Travel() {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, speed * Time.deltaTime, targetMasks)) { 
            transform.position = hit.point;
            OnHit(hit);
        }
        else {
            distanceTravelled += speed * Time.deltaTime;
            if (distanceTravelled + speed * Time.deltaTime >= range) { 
                DespawnBullet(); 
            }
            else {
                transform.position += speed * Time.deltaTime * direction;
            }
        }
    }


    public override void Initialize(float damage, Vector3 spawnPosition, Vector3 direction, float range, float speed, float knockbackStrength, int reflectCount, float shooterScaleAtTimeOfLaunch) {
        trailRenderer.Clear();
        trailRenderer.enabled = false;
        transform.SetPositionAndRotation(spawnPosition, Quaternion.LookRotation(direction));
        trailRenderer.enabled = true;

        this.damage = damage;
        this.direction = direction;
        this.range = range;
        this.speed = speed;
        this.maxReflects = reflectCount;
        this.knockbackStrength = knockbackStrength;
        this.shooterScaleAtTimeOfLaunch = shooterScaleAtTimeOfLaunch;

        launched = false;
        bulletRenderer.enabled = true;
        finished = false;
        distanceTravelled = 0;
        reflects = 0;

        transform.localScale = Vector3.one * shooterScaleAtTimeOfLaunch;
        trailRenderer.widthMultiplier = shooterScaleAtTimeOfLaunch;
    }

    public override void Launch() {
        launched = true;
    }

    
    public override void OnHit(RaycastHit hitData) {
        hitData.collider.gameObject.transform.TryGetComponent(out ITarget target);
        bool hitUnit = target != null;

        hitData.collider.gameObject.TryGetComponent(out SurfaceType st);
        bool hitTerrain = st != null;
        string surfaceType = "";
        bool reflect;

        if (hitUnit) {
            // hit
            target.Hit(new HitDataContainer(this.gameObject, damage, "Bullet"));

            TargetSurface surface = target.Surface;
            switch (surface) {
                case TargetSurface.Metal:
                    SetSparks(hitData);
                    break;
                default:
                    break;
            }

            Knockback(hitData);

            reflect = target.Reflective;
        } 
        else if (hitTerrain) {
            // get surface type (and spawn bullet hole)
            surfaceType = st.SurfaceTypeString;
            reflect = st.Reflective;
            SpawnBulletHole(hitData);
        }
        else {
            reflect = false;
            //KnockbackRigidbody(hitData);
            Debug.Log("Didnt hit unit or terrain");
        }



        if (reflects < maxReflects && reflect) {
            reflects++;
            if (surfaceType != "") { bulletSoundManager.BulletRicochetSound(surfaceType); }
            RicochetBullet(hitData.point, hitData.normal);
        }
        else {
            if (surfaceType != "") { bulletSoundManager.BulletImpactSound(surfaceType); }
            SetSparks(hitData);
            DespawnBullet();
        }
    }

    void Knockback(RaycastHit hitData) {

        // knockback
        hitData.transform.TryGetComponent(out IKnockable kn);
        kn?.AddForce(hitData.point, transform.forward, shooterScaleAtTimeOfLaunch, knockbackStrength);
    }

    void KnockbackRigidbody(RaycastHit hitData) {
        if (hitData.transform.TryGetComponent(out Rigidbody rb)) {
            Debug.Log("FORCE");
            rb.AddForceAtPosition(transform.forward * knockbackStrength * 10f, hitData.point, ForceMode.Impulse);

        }
    }


    void SetSparks(RaycastHit hitData) {
        sparks.Stop();
        sparks.transform.position = hitData.point;
        sparks.transform.rotation = Quaternion.LookRotation(hitData.normal);
        sparks.transform.position += sparks.transform.forward * 0.1f;
        sparks.Play();
    }

    void RicochetBullet(Vector3 point, Vector3 surfaceNormal) {
        Vector3 reflectVector = Vector3.Reflect(transform.forward, surfaceNormal);
        transform.forward = reflectVector;
        direction = reflectVector;

        transform.position += reflectVector * 0.1f;
    }
    void SpawnBulletHole(RaycastHit hitData) {
        BaseBulletHole bbh = bulletHolePooler.Get();
        bbh.transform.SetPositionAndRotation(hitData.point, Quaternion.LookRotation(direction));
        bbh.Initialize(hitData.collider);
    }

    void DespawnBullet() {
        finished = true;
        bulletRenderer.enabled = false;

        DOTween.Sequence()
            .AppendInterval(trailRenderer.time)
            .AppendCallback(() => { bulletPooler.Release(this); });
    }
}
