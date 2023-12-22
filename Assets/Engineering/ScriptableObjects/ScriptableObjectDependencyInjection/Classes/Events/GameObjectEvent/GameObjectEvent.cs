using UnityEngine;

namespace ScriptableObjectDependencyInjection
{
    [CreateAssetMenu(fileName = "GameObjectEvent", menuName = "ScriptableObjects/GameEvent/GameObject")]
    public class GameObjectEvent : ScriptableGameEvent<GameObject> { }
}