using System;
using UnityEngine;

namespace ScriptableObjectDependencyInjection
{
    [CreateAssetMenu(fileName = "StringVariable", menuName = "ScriptableObjects/Variable/String")]
    public class StringVariable : ScriptableVariable<string> { }

    [Serializable]
    public class StringReference : ScriptableReference<StringVariable, string> { }
}