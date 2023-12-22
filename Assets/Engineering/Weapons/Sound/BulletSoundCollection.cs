using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletSoundCollection", menuName = "ScriptableObjects/DataContainers/BulletSoundCollection")]
public class BulletSoundCollection : ScriptableObject
{
    public string bulletImpactSoundPath;
    public string bulletRicochetSoundPath;
}