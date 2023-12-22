using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Decisions/Sprint/Sprint To Walk")]
    public class SprintToWalkDecision : PlayerMovementStateDecision {
        public override bool Decide(PlayerMovementStateMachine machine) {
            if (!machine.IsGrounded() || machine.IsForceUngrounded()) {
                return true;
            }


            if (!machine.Inputs.SprintHold 
                || machine.Inputs.RawMove.y < 0
                //|| machine.CurrentLocalVelocity.z <= 0.0f
                //|| (machine.Inputs.RawMove.x != 0
                || (machine.CurrentLocalVelocity.z <= 0f && Mathf.Abs(machine.CurrentLocalVelocity.x) <= 1.0f))
                { 
                return true;
            }

            return false;
        }
    }
}