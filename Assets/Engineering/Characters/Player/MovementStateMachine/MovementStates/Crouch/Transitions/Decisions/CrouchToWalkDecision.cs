using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Decisions/Crouch/Crouch To Walk")]
    public class CrouchToWalkDecision : PlayerMovementStateDecision
    {
        public override bool Decide(PlayerMovementStateMachine machine) {
            if (machine.Inputs.CrouchHold) { return false; }

            if (machine.TryUncrouch()) {
                machine.SetAnimatorBool("Crouch", false);
                return true;
            }
            return false;
        }
    }
}