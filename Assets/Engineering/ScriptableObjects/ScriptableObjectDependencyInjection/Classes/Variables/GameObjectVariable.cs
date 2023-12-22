using System;
using UnityEngine;

namespace ScriptableObjectDependencyInjection
{
    [CreateAssetMenu(fileName = "GameObjectVariable", menuName = "ScriptableObjects/Variable/GameObject")]
    public class GameObjectVariable : ScriptableVariable<GameObject> { }

    [Serializable]
    public class GameObjectReference : ScriptableReference<GameObjectVariable, GameObject> { }
}