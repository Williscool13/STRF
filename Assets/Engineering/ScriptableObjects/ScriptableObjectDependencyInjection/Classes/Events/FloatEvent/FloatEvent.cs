using UnityEngine;

namespace ScriptableObjectDependencyInjection {
    [CreateAssetMenu(fileName = "FloatEvent", menuName = "ScriptableObjects/GameEvent/Float")]
    public class FloatEvent : ScriptableGameEvent<float> { }
}