using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Actions/Reload/Update")]
    public class ReloadUpdate : PlayerActionStateAction {
        [SerializeField] FloatVariable reloadTimeleft;
        public override void Execute(PlayerActionStateMachine machine) {
            if (reloadTimeleft.Value <= 0) {
                return;
            }

            reloadTimeleft.Value -= Time.deltaTime;
        }
    }
}