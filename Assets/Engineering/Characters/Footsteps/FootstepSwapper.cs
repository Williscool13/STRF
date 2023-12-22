using PlayerFiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class FootstepSwapper : MonoBehaviour
{
    [SerializeField] private SurfaceMaterialProperties[] footstepCollections;
    [SerializeField] private FootstepManager footstepManager;
    [SerializeField] private float terrainCheckRange = 0.5f;

    string currentFootstepMaterialId = "";

    public void CheckLayers() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -footstepManager.transform.up, out hit, terrainCheckRange)) {

            // Mesh Check
            if (hit.transform.GetComponent<SurfaceType>() != null) {
                string targetId = hit.transform.GetComponent<SurfaceType>().SurfaceTypeString;
                if (currentFootstepMaterialId == targetId) { return; }

                footstepManager.SetFootstepId(targetId);
                currentFootstepMaterialId = targetId;
                return;
            }
        }
    }
}
