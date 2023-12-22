using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Crouch/Exit")]
    public class CrouchExit : PlayerMovementStateAction
    {
        public override void Execute(PlayerMovementStateMachine machine) {
            // set camera position to standing
            machine.AimController.Crouching = false;

            machine.SetAnimatorBool("Crouch", false);
        }
    }
}
