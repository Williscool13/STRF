using DG.Tweening;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSoundManager : MonoBehaviour
{
    [SerializeField] BulletSoundCollection bulletSoundCollection;

    public void BulletImpactSound(string surfaceType) {
        EventInstance _bulletImpactSound = FMODUnity.RuntimeManager.CreateInstance(bulletSoundCollection.bulletImpactSoundPath);
        _bulletImpactSound.setParameterByNameWithLabel("SurfaceType", surfaceType);
        _bulletImpactSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        _bulletImpactSound.start();
        _bulletImpactSound.release();
    }
    public void BulletRicochetSound(string surfaceType) {
        EventInstance _bulletRicochetSound = FMODUnity.RuntimeManager.CreateInstance(bulletSoundCollection.bulletRicochetSoundPath);
        _bulletRicochetSound.setParameterByNameWithLabel("SurfaceType", surfaceType);
        _bulletRicochetSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        _bulletRicochetSound.start();
        _bulletRicochetSound.release();
    }
}
