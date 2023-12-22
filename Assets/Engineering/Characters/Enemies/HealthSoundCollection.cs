using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthSoundCollection", menuName = "ScriptableObjects/DataContainers/HealthSoundCollection")]
public class HealthSoundCollection : ScriptableObject
{
    public string hitSoundPath;
    public string deathSoundPath;
}
