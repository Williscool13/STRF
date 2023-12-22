using System;
using UnityEngine;

namespace ScriptableObjectDependencyInjection
{
    [CreateAssetMenu(fileName = "BoolVariable", menuName = "ScriptableObjects/Variable/Bool")]
    public class BoolVariable : ScriptableVariable<bool> { }

    [Serializable]
    public class BoolReference : ScriptableReference<BoolVariable, bool> { }

}