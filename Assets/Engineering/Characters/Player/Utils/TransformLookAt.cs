using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLookAt : MonoBehaviour
{
    [SerializeField] Transform target;
    void Update()
    {
        Vector3 lookDirection = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(lookDirection, transform.root.up);
    }
}
