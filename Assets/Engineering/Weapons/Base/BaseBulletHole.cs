using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class BaseBulletHole : MonoBehaviour
{

    public abstract void Initialize(Collider collidedObject);


    public void SetBulletHolePooler(IObjectPool<BaseBulletHole> pooler) { this.bulletHolePooler = pooler; }
    
    protected IObjectPool<BaseBulletHole> bulletHolePooler;
    protected virtual void Release() {
        bulletHolePooler.Release(this);
    }
}
