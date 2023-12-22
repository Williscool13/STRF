using KinematicCharacterController;
using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScaleManager : MonoBehaviour, IScalable
{
    [SerializeField] KinematicCharacterMotor motor;
    [SerializeField] Transform model;
    [SerializeField] float baseScale;
    [SerializeField] float maxScale;
    [SerializeField] float minScale;
    [SerializeField] float scaleIncrement = 0.5f;
    [SerializeField] Transform rootTransform;
    [SerializeField] LayerMask ceilingMask;

    [SerializeField] FloatVariable playerScale;

    float currScale;
    float baseRadius;
    float baseHeight;
    float baseStepHeight;

    private void Start() {
        baseRadius = motor.Capsule.radius;
        baseHeight = motor.Capsule.height;
        baseStepHeight = motor.MaxStepHeight;
        currScale = 1;
        playerScale.Value = currScale;
    }


    #region Scale
    [Button("Scale Up")]
    [HorizontalGroup("Scale")]
    public void ScaleIncrementUp() {
        if (Mathf.Approximately(currScale, maxScale) || currScale > maxScale) return;

        if (Physics.Raycast(rootTransform.position + rootTransform.up * (baseHeight * currScale), rootTransform.up, out RaycastHit hit, baseHeight * scaleIncrement * 1.1f, ceilingMask)) { return; }

        SetScaleDiff(scaleIncrement);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PlayerSize", scaleIncrement);
    }
    [Button("Scale Down")]
    [HorizontalGroup("Scale")]
    public void ScaleIncrementDown() {
        if (Mathf.Approximately(currScale, minScale) || currScale < minScale) return;
        SetScaleDiff(-scaleIncrement);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PlayerSize", -scaleIncrement);
    }

    void SetScaleDiff(float diff) {
        currScale += diff;
        currScale = Mathf.Clamp(currScale, minScale, maxScale);
        motor.SetCapsuleDimensions(baseRadius * currScale, baseHeight * currScale, baseHeight * currScale / 2);
        model.localScale = Vector3.one * (baseScale * currScale);

        float heightDiff = diff / 2 + 0.05f;
        motor.SetPosition(rootTransform.position + rootTransform.up * heightDiff);

        motor.MaxStepHeight = baseStepHeight * currScale;

        playerScale.Value = currScale;
    }

    public void ResetScale() {
        if (Mathf.Approximately(currScale, 1)) return;
        SetScaleDiff(1 - currScale);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PlayerSize", 1 - currScale);
    }
    public void SetScale(float val) {
        SetScaleDiff(val - currScale);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PlayerSize", val - currScale);
    }

    public float GetCurrentScale() {
        return currScale;
    }
    #endregion
}


public interface IScalable
{
    public void ScaleIncrementUp();
    public void ScaleIncrementDown();
    public void ResetScale();
    public void SetScale(float val);
    public float GetCurrentScale();
}