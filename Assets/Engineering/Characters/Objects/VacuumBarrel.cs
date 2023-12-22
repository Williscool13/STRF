using Combat;
using DG.Tweening;
using FMOD.Studio;
using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumBarrel : MonoBehaviour, ITarget
{
    [SerializeField] bool reflective;
    [SerializeField] ParticleSystem vacuumEffect;
    [SerializeField] float vacuumRange = 5f;
    [SerializeField] float vacuumForce = 30f;
    [SerializeField] NullEvent onBarrelImplode;
    [SerializeField] string vacuumSoundPath;
    public TargetSurface Surface => TargetSurface.Metal;

    public bool Reflective => reflective;

    public event EventHandler<HitDataContainer> OnEnemyHit;


    [Button("Vacuum")]
    void BlowUp() {
        Hit(new HitDataContainer(this.gameObject, 0, "hi!"));
    }
    public void Hit(HitDataContainer damage) {
        Implode();
    }

    void Implode() {
        vacuumEffect.Play();
        transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => {
            Destroy(gameObject);
        });

        Collider[] units = Physics.OverlapSphere(transform.position, vacuumRange);
        Debug.Log("Barrel hit " + units.Length + " units");
        foreach (Collider unit in units) {
            
            if (unit.TryGetComponent(out IVacuumable vac)) {
                vac.Vacuum(transform.position, (transform.position - unit.transform.position), 1, vacuumForce);
                Debug.Log("Vacuuming " + unit.name);
            }
        }
        if (onBarrelImplode != null) onBarrelImplode.Raise(null);

        EventInstance eventInstance = FMODUnity.RuntimeManager.CreateInstance(vacuumSoundPath);
        eventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
        eventInstance.start();
        eventInstance.release();

        GetComponent<Collider>().enabled = false;
    }

    private void Update() {
        if (transform.position.y < -200) Destroy(gameObject);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, vacuumRange);
    }
}


public interface IVacuumable
{
    void Vacuum(Vector3 point, Vector3 normalizedForceDirection, float sourceScale, float forceStrength);
}