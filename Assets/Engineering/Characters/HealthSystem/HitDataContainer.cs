using System;
using UnityEngine;

[Serializable]
public class HitDataContainer
{
    public float RawDamage { get; set; }
    public int DamageMultiplier { get; set; }
    public string DamageName { get; set; }
    public GameObject DamageSender { get; set; }

    public HitDataContainer() { }
    public HitDataContainer(GameObject sender, float damage, string name) {
        this.RawDamage = damage;
        this.DamageMultiplier = 1;
        this.DamageName = name;
        this.DamageSender = sender;

    }

    public int GetTotalDamage() {
        return (int)this.RawDamage * this.DamageMultiplier;
    }

}
