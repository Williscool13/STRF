using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Decisions/Walk/Walk To Fall")]
    public class WalkToFallDecision : PlayerMovementStateDecision {
        public override bool Decide(PlayerMovementStateMachine Machine) {
            if (!Machine.IsGrounded()) {
                return true;
            }
            return false;
        }
    }
}