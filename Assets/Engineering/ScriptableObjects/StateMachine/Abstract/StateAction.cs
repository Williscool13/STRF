using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FiniteStateMachine {
    public abstract class StateAction<StateMachine> : ScriptableObject
{
        public abstract void Execute(StateMachine machine);
    }
}