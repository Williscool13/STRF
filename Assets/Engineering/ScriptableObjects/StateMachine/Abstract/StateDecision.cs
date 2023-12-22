using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FiniteStateMachine
{
    public abstract class StateDecision<StateMachine> : ScriptableObject
    {
        public abstract bool Decide(StateMachine Machine);
    }

}