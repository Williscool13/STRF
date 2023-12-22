using UnityEngine;
namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Common/Jump")]
    public class JumpUpdate : PlayerMovementStateAction
    {
        [SerializeField] float movementVelocityMultiplier = 3.0f;
        public override void Execute(PlayerMovementStateMachine machine) {
            if (machine.Inputs.JumpPress && machine.CanJump()) {
                machine.JumpRequested = true;
                machine.TimeSinceJumpRequested = 0f;
                machine.JumpForwardMultiplier = movementVelocityMultiplier;
            }
        }
    }

}