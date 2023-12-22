using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class BaseBullet : MonoBehaviour
{


    public abstract void Initialize(float damage, Vector3 spawnPosition, Vector3 direction, float range, float speed, float knockbackStrength, int reflectCount, float playerScaleAtTimeOfLaunch);
    public abstract void Launch();
    public abstract void OnHit(RaycastHit hitData);

    public void SetBulletHolePooler(IObjectPool<BaseBulletHole> pool) { this.bulletHolePooler = pool; }
    public void SetBulletPooler(IObjectPool<BaseBullet> pool) { this.bulletPooler = pool; }

    protected IObjectPool<BaseBullet> bulletPooler;
    protected IObjectPool<BaseBulletHole> bulletHolePooler;
    protected void Release() {
        bulletPooler.Release(this);
    }
}
