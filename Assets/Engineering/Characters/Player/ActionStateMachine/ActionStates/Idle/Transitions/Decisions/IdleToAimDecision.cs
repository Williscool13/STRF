using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Decisions/Idle/Idle to Aim")]

    public class IdleToAimDecision : PlayerActionStateDecision
    {
        [SerializeField] List<PlayerMovementState> invalidMovements;
        public override bool Decide(PlayerActionStateMachine Machine) {
            if (invalidMovements.Contains(Machine.PlayerMovementStateMachine.CurrentState)) { return false; }
            if (Machine.Inputs.AimHold) { return true; }
            return false;
        }
    }
}