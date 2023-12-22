using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FiniteStateMachine
{
    public abstract class BaseState<StateMachine> : ScriptableObject
    {
        public virtual void Enter(StateMachine machine) { }
        public virtual void Execute(StateMachine machine) { }
        public virtual void Exit(StateMachine machine) { }
    }

}