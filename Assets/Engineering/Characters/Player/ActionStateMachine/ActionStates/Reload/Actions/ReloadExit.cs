using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Actions/Reload/Exit")]
    public class ReloadExit : PlayerActionStateAction {
        [SerializeField] BoolVariable isReloading;
        public override void Execute(PlayerActionStateMachine machine) {
            machine.PlayerLoadoutManager.GetCurrentWeapon().ReloadEnd();

            isReloading.Value = false;
        }
    }
}