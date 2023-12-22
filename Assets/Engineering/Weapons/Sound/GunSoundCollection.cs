using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunSoundCollection", menuName = "ScriptableObjects/DataContainers/GunSoundCollection")]
public class GunSoundCollection : ScriptableObject
{
    public string fireSoundPath;
    public string reloadSoundPath;
    public string adsEnterSoundPath;
    public string adsExitSoundPath;
    public string equipSoundPath;
    public string unequipSoundPath;
}
