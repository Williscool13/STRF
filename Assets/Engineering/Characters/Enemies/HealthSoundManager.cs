using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSoundManager : MonoBehaviour
{
    [SerializeField] HealthSoundCollection healthSoundCollection;

    public void HitSound() {
        EventInstance _bulletImpactSound = FMODUnity.RuntimeManager.CreateInstance(healthSoundCollection.hitSoundPath);
        _bulletImpactSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        _bulletImpactSound.start();
        _bulletImpactSound.release();
    }
    public void DeathSound() {
        EventInstance _bulletRicochetSound = FMODUnity.RuntimeManager.CreateInstance(healthSoundCollection.deathSoundPath);
        _bulletRicochetSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        _bulletRicochetSound.start();
        _bulletRicochetSound.release();
    }
}
