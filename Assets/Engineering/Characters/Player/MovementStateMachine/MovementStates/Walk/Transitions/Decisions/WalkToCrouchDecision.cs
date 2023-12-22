using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Decisions/Walk/Walk To Crouch")]
    public class WalkToCrouchDecision : PlayerMovementStateDecision
    {
        public override bool Decide(PlayerMovementStateMachine machine) {
            return machine.Inputs.CrouchHold && !machine.Inputs.SprintHold && machine.IsGrounded();
        }
    }
}
