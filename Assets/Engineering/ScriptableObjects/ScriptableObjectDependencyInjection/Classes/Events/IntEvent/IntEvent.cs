using UnityEngine;
namespace ScriptableObjectDependencyInjection
{
    [CreateAssetMenu(fileName = "IntEvent", menuName = "ScriptableObjects/GameEvent/Int")]
    public class IntEvent : ScriptableGameEvent<int> { }
}