using System;
using UnityEngine;


namespace ScriptableObjectDependencyInjection
{
    [CreateAssetMenu(fileName = "FloatVariable", menuName = "ScriptableObjects/Variable/Float")]
    public class FloatVariable : ScriptableVariable<float> { }

    [Serializable]
    public class FloatReference : ScriptableReference<FloatVariable, float> { }
}