using UnityEngine;

namespace ScriptableObjectDependencyInjection
{
    [CreateAssetMenu(fileName = "HitDataContainerEvent", menuName = "ScriptableObjects/GameEvent/HitDataContainerEvent")]
    public class HitDataContainerEvent : ScriptableGameEvent<HitDataContainer> { }
}