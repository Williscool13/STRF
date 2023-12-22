using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Decisions/Reload/Reload To Idle")]
    public class ReloadToIdleDecision : PlayerActionStateDecision {
        [SerializeField] FloatReference reloadTimeleft;
        public override bool Decide(PlayerActionStateMachine machine) {
            return reloadTimeleft.Value <= 0.0f;
        }
    }
}