using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Actions/Swap/Update")]
    public class SwapUpdate : PlayerActionStateAction {
        [SerializeField] FloatVariable transformSwapTimeleft;
        [SerializeField] FloatVariable swapTimeleft;
        [SerializeField] BoolVariable weaponSwapped;
        public override void Execute(PlayerActionStateMachine machine) {
            
            if (!weaponSwapped.Value) {
                if (transformSwapTimeleft.Value <= 0) {
                    machine.SwapWeaponTransforms();
                    weaponSwapped.Value = true;
                }

                transformSwapTimeleft.Value -= Time.deltaTime;
            }

            if (swapTimeleft.Value <= 0) {
                return;
            }

            swapTimeleft.Value -= Time.deltaTime;

        }
    }
}