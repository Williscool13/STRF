using Combat;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PhysicsObject : MonoBehaviour, ITarget, IKnockable, IScalable, IVacuumable
{
    [SerializeField] Rigidbody rb;
    [SerializeField] bool reflective;


    [HideIf("startsKinematic")]
    [SerializeField] bool isKinematic; 
    [HideIf("isKinematic")]
    [SerializeField] bool startsKinematic;
    [InfoBox("The force required to push an object. If the object startsKinematic, it will be set to false when this threshold is met for the first time.")]
    [SerializeField] float forceThreshold = 0.5f;


    [Title("Scale Change Prameters")]
    [SerializeField] float scaleIncrement = 0.2f;
    [SerializeField] float maxScale = 2.0f;
    [SerializeField] float minScale = 0.2f;
    [SerializeField] float objectHeight = 1.0f;


    public TargetSurface Surface => TargetSurface.Metal;
    public bool Reflective => reflective;
    [SerializeField] LayerMask ceilingMask;

    [Title("Sound")]
    [SerializeField] HealthSoundManager healthSoundManager;

    public event EventHandler<HitDataContainer> OnEnemyHit;


    float currScale = 1f;
    Vector3 baseScale;

    private void Start() {
        if (startsKinematic) rb.isKinematic = true;
        baseScale = transform.localScale;

        if (!rb) {
            rb = GetComponent<Rigidbody>();
        }
    }

    public void AddForce(Vector3 point, Vector3 normalizedForceDirection, float sourceScale, float forceStrength) {
        Vector3 finalForce;
        float proposedStrengthModifier = Mathf.Pow(sourceScale, 3) / (baseScale.GetVectorValuesAverage() * currScale);

        if (proposedStrengthModifier < forceThreshold) proposedStrengthModifier = 0f; 
        else { 
            if (!isKinematic) rb.isKinematic = false; 
        }
        finalForce = normalizedForceDirection * forceStrength * proposedStrengthModifier;
        
        rb.AddForceAtPosition(finalForce, point, ForceMode.Impulse);
    }




    public void Hit(HitDataContainer damage) {
        if (healthSoundManager == null) { return; }
        healthSoundManager.HitSound();
    }


    #region Scale
    [Button("Scale Up")]
    [HorizontalGroup("Scale")]
    public void ScaleIncrementUp() {
        if (Mathf.Approximately(currScale, maxScale) || currScale > maxScale) return;

        if (Physics.Raycast(transform.position + transform.up * (currScale * objectHeight) , transform.up, out RaycastHit hit, objectHeight * scaleIncrement * 1.1f, ceilingMask)) { return; }

        currScale += scaleIncrement;
        transform.localScale = baseScale * currScale;

        rb.mass = Mathf.Sqrt((baseScale.GetVectorValuesAverage() - 0.2f) * currScale);
    }

    [Button("Scale Down")]
    [HorizontalGroup("Scale")]
    public void ScaleIncrementDown() {
        if (Mathf.Approximately(currScale, minScale) || currScale < minScale) return;

        currScale -= scaleIncrement;
        transform.localScale = baseScale * currScale;

        rb.mass = Mathf.Sqrt(baseScale.GetVectorValuesAverage() * currScale);
    }

    public void ResetScale() {
        SetScale(1);
    }

    public void SetScale(float mult) {
        currScale = mult;
        transform.localScale = baseScale * currScale;

        rb.mass = Mathf.Sqrt(baseScale.GetVectorValuesAverage() * currScale);
    }

    public float GetCurrentScale() {
        return currScale;
    }

    [SerializeField] bool vacuumable;
    [ShowIf("vacuumable")][SerializeField] bool scaleAffectsVacuum;
    [ShowIf("vacuumable")][SerializeField] bool keepConstraints;
    public void Vacuum(Vector3 point, Vector3 normalizedForceDirection, float sourceScale, float forceStrength) {
        if (!vacuumable) return;

        if (transform.TryGetComponent(out Rigidbody rb)) {
            if (!keepConstraints) {
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            }
            if (scaleAffectsVacuum) forceStrength /= Mathf.Pow(GetCurrentScale(), 6);
            //if (scalable != null) forceStrength *= 1 / scalable.GetCurrentScale();
            rb.AddForceAtPosition(normalizedForceDirection * forceStrength, point, ForceMode.Impulse);
            Debug.Log("VACUUMED with force " + forceStrength);
        }
    }
    #endregion
}
