using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceType : MonoBehaviour
{
    [SerializeField] private bool reflective;
    [SerializeField] private SurfaceMaterialProperties surfaceMaterialProperties;
    public bool Reflective => reflective;
    public string SurfaceTypeString => surfaceMaterialProperties.materialId;

}