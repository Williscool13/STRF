using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Decisions/Aim/Aim To Swap")]
    public class AimToSwapDecision : PlayerActionStateDecision {
        public override bool Decide(PlayerActionStateMachine Machine) {
            if (Machine.Inputs.SwapPress) {
                return true;
            }

            return false;
        }
    }
}