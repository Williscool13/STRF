using Combat;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SizeModifyingBullet : BaseBullet
{
    [SerializeField] LayerMask targetMasks;

    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] MeshRenderer bulletRenderer;

    [SerializeField] ParticleSystem sparks;

    [SerializeField] BulletSoundManager bulletSoundManager;

    [SerializeField] SizeChange sizeChange;

    int maxReflects = 0;

    Vector3 direction;
    float damage;
    float range;
    float speed;
    float knockbackStrength;

    bool launched;
    bool finished;
    float distanceTravelled;

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

    public override void Initialize(float damage, Vector3 spawnPosition, Vector3 direction, float range, float speed, float knockbackStrength, int reflectCount, float playerScaleAtTimeOfLaunch) {
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

        launched = false;
        bulletRenderer.enabled = true;
        finished = false;
        distanceTravelled = 0;
        reflects = 0;
    }

    public override void Launch() {
        launched = true;
    }

    public override void OnHit(RaycastHit hitData) {
        hitData.transform.TryGetComponent(out ITarget target);
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
                    surfaceType = "Metal";
                    SetSparks(hitData); 
                    break;
                default:
                    break;
            }

            if (hitData.transform.TryGetComponent(out IScalable scalable)) {
                if (sizeChange == SizeChange.Up) scalable.ScaleIncrementUp();
                else scalable.ScaleIncrementDown();
            }

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
            Debug.LogError("Didn't hit either a unit or terrain name: " + hitData.transform.name);
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

    void SetSparks(RaycastHit hitData) {
        sparks.Stop();
        sparks.transform.position = hitData.point;
        sparks.transform.rotation = Quaternion.LookRotation(hitData.normal);
        sparks.transform.position += sparks.transform.forward * 0.1f;
        sparks.Play();
    }

    void DespawnBullet() {
        finished = true;
        bulletRenderer.enabled = false;

        DOTween.Sequence()
            .AppendCallback(() => { bulletPooler.Release(this); });
    }

    public enum SizeChange {
        Up,
        Down
    }
}


