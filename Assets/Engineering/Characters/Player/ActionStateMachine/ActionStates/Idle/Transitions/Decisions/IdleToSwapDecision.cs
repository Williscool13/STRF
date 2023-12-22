using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Decisions/Idle/Idle To Swap")]
    public class IdleToSwapDecision : PlayerActionStateDecision
    {
        [SerializeField] List<PlayerMovementState> validMovementStates = new List<PlayerMovementState>();
        public override bool Decide(PlayerActionStateMachine machine) {
            if (!machine.Inputs.SwapPress) {
                return false;
            }
            if (validMovementStates.Contains(machine.PlayerMovementStateMachine.CurrentState)) {
                return true;
            }


            return false;
        }
    }


}