using PlayerFiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FiniteStateMachine
{
    public abstract class BaseStateTransition<StateMachine> : ScriptableObject
    {
        /*public PlayerMovementStateDecision decision;
        public PlayerMovementState trueState;
        public PlayerMovementState falseState;*/

        public virtual void Execute(StateMachine machine) {
            /*if (decision.Decide(machine)) {
                if (machine.CurrentState != trueState) {
                    machine.CurrentState.Exit(machine);
                    machine.CurrentState = trueState;
                    machine.CurrentState.Enter(machine);
                }
            }
            else {
                if (machine.CurrentState != falseState) {
                    machine.CurrentState.Exit(machine);
                    machine.CurrentState = falseState;
                    machine.CurrentState.Enter(machine);
                }
            }*/
        }
    }
}