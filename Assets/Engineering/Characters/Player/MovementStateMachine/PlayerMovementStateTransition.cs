using FiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Transition")]
    public sealed class PlayerMovementStateTransition : BaseStateTransition<PlayerMovementStateMachine>
    {
        public PlayerMovementStateDecision decision;
        public PlayerMovementState trueState;
        public PlayerMovementState falseState;

        public override void Execute(PlayerMovementStateMachine machine) {
            if (decision.Decide(machine)) {
                if (trueState is not MovementState_Remain) {
                    machine.TransitionState(trueState);
                }
            }
            else {
                if (falseState is not MovementState_Remain) {
                    machine.TransitionState(falseState);
                } 
            }
        }    
    }
}