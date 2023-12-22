using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Decisions/Aim/Aim To Idle")]
    public class AimToIdleDecision : PlayerActionStateDecision {
        [SerializeField] List<PlayerMovementState> invalidMovements;
        public override bool Decide(PlayerActionStateMachine Machine) {
            if (invalidMovements.Contains(Machine.PlayerMovementStateMachine.CurrentState)) {
                return true;
            }

            if (!Machine.Inputs.AimHold) {
                return true;
            }

            return false;
        }
    }
}

