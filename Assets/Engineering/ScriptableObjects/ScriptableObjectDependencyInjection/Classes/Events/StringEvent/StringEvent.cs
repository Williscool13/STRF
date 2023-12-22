using UnityEngine;
namespace ScriptableObjectDependencyInjection
{
    [CreateAssetMenu(fileName = "StringEvent", menuName = "ScriptableObjects/GameEvent/String")]
    public class StringEvent : ScriptableGameEvent<string> { }
}