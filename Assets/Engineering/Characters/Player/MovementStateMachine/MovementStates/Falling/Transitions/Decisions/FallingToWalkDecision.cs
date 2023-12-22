using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Decisions/Falling/Falling To Walk")]
    public class FallingToWalkDecision : PlayerMovementStateDecision
    {
        public override bool Decide(PlayerMovementStateMachine Machine) {
            if (Machine.IsGrounded()) {
                return true;
            }
            return false;
        }
    }
}