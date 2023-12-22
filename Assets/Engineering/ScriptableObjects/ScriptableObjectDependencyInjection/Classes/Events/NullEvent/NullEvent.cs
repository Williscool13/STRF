using UnityEngine;
namespace ScriptableObjectDependencyInjection
{
    [CreateAssetMenu(fileName = "NullEvent", menuName = "ScriptableObjects/GameEvent/Null")]
    public class NullEvent : ScriptableGameEvent<object> { }
}