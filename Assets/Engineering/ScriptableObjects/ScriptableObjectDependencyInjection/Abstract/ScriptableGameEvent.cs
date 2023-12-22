using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectDependencyInjection
{
    public abstract class ScriptableGameEvent<T> : ScriptableObject
    {
        private List<IGameEventListener<T>> listeners = new();

        public void Raise(T eventData) {
            Debug.Log($"[{name}] GameEvent Raised (Data: {eventData})");
            for (int i = listeners.Count - 1; i >= 0; i--) {
                listeners[i].OnEventRaised(eventData);
            }
        }

        public void RegisterListener(IGameEventListener<T> listener) {
            listeners.Add(listener);
        }

        public void UnregisterListener(IGameEventListener<T> listener) {
            listeners.Remove(listener);
        }
    }
}