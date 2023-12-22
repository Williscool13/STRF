using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformCopy : MonoBehaviour
{
    [SerializeField] private Transform targetTrans;
    [SerializeField] private float forwardOffset = 0f;
    public void SetTarget(Transform tar) {
        targetTrans = tar;
    }
    void Update()
    {
        transform.position = targetTrans.position + targetTrans.forward * forwardOffset;
        transform.rotation = targetTrans.rotation;
    }
}
