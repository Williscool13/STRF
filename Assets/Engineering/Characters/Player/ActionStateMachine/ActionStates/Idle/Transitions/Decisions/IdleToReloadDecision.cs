using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine {
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Decisions/Idle/Idle To Reload")]
    public class IdleToReloadDecision : PlayerActionStateDecision
    {
        [SerializeField] List<PlayerMovementState> validMovementStates = new List<PlayerMovementState>();
        public override bool Decide(PlayerActionStateMachine machine) {
            if (!machine.Inputs.ReloadPress) {
                return false;
            }

            if (!machine.PlayerLoadoutManager.GetCurrentWeapon().CanReload()) {
                return false;
            }

            if (!validMovementStates.Contains(machine.PlayerMovementStateMachine.CurrentState)) {
                return false;
            }


            return true;
        }

    }

}