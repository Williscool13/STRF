using System;
using UnityEngine;

namespace ScriptableObjectDependencyInjection
{
    [CreateAssetMenu(fileName = "IntegerVariable", menuName = "ScriptableObjects/Variable/Integer")]
    public class IntegerVariable : ScriptableVariable<int> { }

    [Serializable]
    public class IntegerReference : ScriptableReference<IntegerVariable, int> { }
}