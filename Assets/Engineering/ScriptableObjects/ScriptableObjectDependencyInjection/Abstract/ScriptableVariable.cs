using UnityEngine;

namespace ScriptableObjectDependencyInjection
{
    /// <summary>
    /// Extensible base class for ScriptableVariables.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ScriptableVariable<T> : ScriptableObject
    {
        [SerializeField] private T value;
        public T Value {
            get { return value; }
            set { this.value = value; }
        }
    }
}