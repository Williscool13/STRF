using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Falling/Exit")]
    public class FallingExit : PlayerMovementStateAction
    {
        public override void Execute(PlayerMovementStateMachine machine) {
            machine.SetAnimatorBool("Falling", false);
        }
    }

}