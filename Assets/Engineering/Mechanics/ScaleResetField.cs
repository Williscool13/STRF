using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleResetField : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out IScalable sca)) sca.ResetScale();
    }
}
