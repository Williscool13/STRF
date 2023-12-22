using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Decisions/Swap/Swap To Aim")]
    public class SwapToAim : PlayerActionStateDecision {
        [SerializeField] FloatReference swapTimeleft;
        public override bool Decide(PlayerActionStateMachine machine) {
            if (swapTimeleft.Value <= 0.0f) {
                return true;
            }

            return false;
        }
    }
}