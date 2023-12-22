using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Decisions/Walk/Walk To Sprint")]
    public class WalkToSprintDecision : PlayerMovementStateDecision {
        [SerializeField] List<PlayerActionState> validActionStates = new List<PlayerActionState>();
        public override bool Decide(PlayerMovementStateMachine machine) {
            // cant sprint if not grounded
            if (!machine.IsGrounded() || machine.IsForceUngrounded()){
                return false;
            }

            // movement constraints on sprint
            if (machine.Inputs.SprintHold // holding sprint
                && !(machine.Inputs.RawMove.y < 0) // not holding backwads
                //&& machine.Inputs.RawMove.y > 0 // holding forward
                //&& machine.Inputs.RawMove.x == 0 // not moving left or right
                && (machine.CurrentLocalVelocity.z > 0f || Mathf.Abs(machine.CurrentLocalVelocity.x) >= 1.0f)) // moving forward
                {
                if (validActionStates.Contains(machine.PlayerActionStateMachine.CurrentState)) {
                    return true;
                }
            }
            return false;
        }
    }
}
