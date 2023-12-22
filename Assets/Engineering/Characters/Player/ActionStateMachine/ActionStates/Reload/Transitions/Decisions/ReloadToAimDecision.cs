using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Decisions/Reload/Reload To Aim")]
    public class ReloadToAimDecision : PlayerActionStateDecision
    {
        [SerializeField] FloatReference reloadTimeleft;
        public override bool Decide(PlayerActionStateMachine machine) {
            if (machine.Inputs.AimHold && reloadTimeleft.Value <= 0.0f) {
                return true;
            }
            return false;
        }
    }
}