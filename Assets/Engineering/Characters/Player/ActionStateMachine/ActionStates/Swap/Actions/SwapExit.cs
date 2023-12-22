using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Actions/Swap/Exit")]
    public class SwapExit : PlayerActionStateAction {
        [SerializeField] BoolVariable isSwapping;
        public override void Execute(PlayerActionStateMachine machine) {
            isSwapping.Value = false;
        }
    }
}