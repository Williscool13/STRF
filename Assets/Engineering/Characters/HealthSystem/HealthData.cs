using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthData", menuName = "ScriptableObjects/DataContainers/HealthData")]
public class HealthData : ScriptableObject
{
    public string unitName;
    public int maxHealth;
}
