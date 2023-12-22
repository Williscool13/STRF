using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSoundManager : MonoBehaviour
{
    [SerializeField] GunSoundCollection gunSoundCollection;

    public void FireSound() { 
        EventInstance _fireSound = FMODUnity.RuntimeManager.CreateInstance(gunSoundCollection.fireSoundPath);
        _fireSound.start();
        _fireSound.release();

    }
    public void ReloadSound() { 
        EventInstance _reloadSound = FMODUnity.RuntimeManager.CreateInstance(gunSoundCollection.reloadSoundPath);
        _reloadSound.start();
        _reloadSound.release();
    }
    public void ADSEnterSound() {
        EventInstance _adsEnterSound = FMODUnity.RuntimeManager.CreateInstance(gunSoundCollection.adsEnterSoundPath);
        _adsEnterSound.start();
        _adsEnterSound.release();
    }
    public void ADSExitSound() {
        EventInstance _adsExitSound = FMODUnity.RuntimeManager.CreateInstance(gunSoundCollection.adsExitSoundPath);
        _adsExitSound.start(); 
        _adsExitSound.release(); 
    }
}
