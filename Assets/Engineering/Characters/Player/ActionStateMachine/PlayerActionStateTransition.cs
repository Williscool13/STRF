using FiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Transition")]
    public sealed class PlayerActionStateTransition : BaseStateTransition<PlayerActionStateMachine> {


        public PlayerActionStateDecision decision;
        public PlayerActionState trueState;
        public PlayerActionState falseState;

        public override void Execute(PlayerActionStateMachine machine) {
            if (decision.Decide(machine)) {
                if (trueState is not ActionState_Remain) {
                    machine.TransitionState(trueState);
                }
            }
            else {
                if (falseState is not ActionState_Remain) {
                    machine.TransitionState(falseState);
                }
            }
        }
    }

}
