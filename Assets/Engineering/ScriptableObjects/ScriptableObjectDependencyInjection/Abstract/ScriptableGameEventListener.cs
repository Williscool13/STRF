using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectDependencyInjection
{
    public abstract class ScriptableGameEventListener<T, GameEvent> : MonoBehaviour, IGameEventListener<T> where GameEvent : ScriptableGameEvent<T>
    {
        //public ScriptableGameEvent<T> Event;
        public GameEvent Event;
        public UnityEvent<T> Response;


        bool automaticEnable = true;
        bool registered = false;

        #region Manual Enable/Disable
        public void SetAutomaticEnable(bool automaticEnable) {
            this.automaticEnable = automaticEnable;
        }

        public void EnableListener() {
            if (registered) return;
            Event.RegisterListener(this);

            registered = true;
        }

        public void DisableListener() {
            if (!registered) return;
            Event.UnregisterListener(this);
            registered = false;
        }
        #endregion


        private void OnEnable() {
            if (!automaticEnable) return;
            Event.RegisterListener(this);
            registered = true;
        }

        private void OnDisable() {
            if (!automaticEnable) return;
            Event.UnregisterListener(this);
            registered = false;
        }

        public void OnEventRaised(T eventData) {
            Response.Invoke(eventData);
        }
    }
}

public interface IGameEventListener<T>
{
    void OnEventRaised(T eventData);
}