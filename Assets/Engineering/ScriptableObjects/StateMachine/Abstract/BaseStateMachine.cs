using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FiniteStateMachine
{
    public abstract class BaseStateMachine : MonoBehaviour {

        //[SerializeField] private BaseState<BaseStateMachine> _initialState;

        public virtual void Awake() {
            //CurrentState = _initialState;
        }

        //public BaseState<BaseStateMachine> CurrentState { get; set; }

        public virtual void Update() {
            //CurrentState.Execute(this);
        }
    }


}