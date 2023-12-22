using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScaleManager : MonoBehaviour, IScalable
{
    [Title("Scalable")]
    [InfoBox("If false, This component will be destroyed. Scale causes hitboxes to behave poorly")]
    [SerializeField] bool isScalable = true;

    [Title("Scale Properties")]
    [ShowIf("isScalable")][SerializeField] Transform model;
    [ShowIf("isScalable")][SerializeField] float baseScale;
    [ShowIf("isScalable")][SerializeField] float maxScale;
    [ShowIf("isScalable")][SerializeField] float minScale;
    [ShowIf("isScalable")][SerializeField] float scaleIncrement = 0.5f;
    [ShowIf("isScalable")][SerializeField] Transform rootTransform;
    [ShowIf("isScalable")][SerializeField] CapsuleCollider capCol;
    [ShowIf("isScalable")][SerializeField] LayerMask ceilingMask;

    float currScale;
    float baseRadius;
    float baseHeight;

    public float GetCurrentScale() {
        return currScale;
    }

    public void ResetScale() {
        if (Mathf.Approximately(currScale, 1)) return;
        SetScaleDiff(1 - currScale);
    }
    [Button("Scale Down")]
    public void ScaleIncrementDown() {
        if (Mathf.Approximately(currScale, minScale) || currScale < minScale) return;
        SetScaleDiff(-scaleIncrement);
    }
    [Button("Scale Up")]
    public void ScaleIncrementUp() {
        if (Mathf.Approximately(currScale, maxScale) || currScale > maxScale) return;

        if (Physics.Raycast(rootTransform.position + rootTransform.up * (baseHeight * currScale), rootTransform.up, out RaycastHit hit, baseHeight * scaleIncrement * 1.1f, ceilingMask)) { return; }

        SetScaleDiff(scaleIncrement);
    }

    public void SetScale(float val) {
        if (Mathf.Approximately(currScale, 1)) return;
        SetScaleDiff(1 - currScale);
    }


    void SetScaleDiff(float diff) {
        currScale += diff;
        currScale = Mathf.Clamp(currScale, minScale, maxScale);
        model.localScale = Vector3.one * (baseScale * currScale);
        float heightDiff = diff / 2 + 0.05f;

        capCol.height = baseHeight * currScale;
        capCol.radius = baseRadius * currScale;
        capCol.center = Vector3.up * (baseHeight * currScale / 2);

        rootTransform.position = rootTransform.position +  rootTransform.up * heightDiff;
    }

    private void Start() {
        if (!isScalable) {
            Destroy(GetComponent<Rigidbody>());
            Destroy(this);
            return;
        }


        baseHeight = capCol.height;
        baseRadius = capCol.radius;
        currScale = rootTransform.localScale.x;
    }
}
