using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Actions/Swap/Enter")]
    public class SwapEnter : PlayerActionStateAction {
        [SerializeField] bool animBasedSwapTime = false;

        [SerializeField] BoolVariable isSwapping;
        [SerializeField] FloatVariable transformSwapTimeleft;
        [SerializeField] FloatVariable swapTimeLeft;
        [SerializeField] BoolVariable weaponSwapped;
        [ShowIf("animBasedSwapTime")]
        [SerializeField] AnimationClip anim;
        [ShowIf("@!animBasedSwapTime")]
        [SerializeField] FloatReference swapTime;
        public override void Execute(PlayerActionStateMachine machine) {
            isSwapping.Value = true;
            weaponSwapped.Value = false;
            float overallTime;
            if (animBasedSwapTime) {
                // 3 is a hardcoded value from the animation
                overallTime = anim.length / 3.0f;
            } else {
                overallTime = swapTime.Value;
            }
            transformSwapTimeleft.Value = overallTime * 0.7f;
            swapTimeLeft.Value = overallTime + 0.2f;

            machine.SetAnimatorTrigger("Swap");

            machine.SwapWeaponMovementFPS(overallTime);
        }
    }
}