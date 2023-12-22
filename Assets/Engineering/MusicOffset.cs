using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicOffset : MonoBehaviour
{
    Vector3 offset = new Vector3(1.0f, 3.0f, 1.0f);
    Transform target;
    void Start()
    {
        target = transform.root;
        transform.parent = null;
    }

    void Update()
    {
        transform.position = target.position + offset;
    }
}
