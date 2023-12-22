using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(fileName = "AimExit", menuName = "Finite State Machine/Player Action/Actions/Aim/Aim Exit")]
    public class AimExit : PlayerActionStateAction {
        [SerializeField] private BoolVariable isAiming;
        public override void Execute(PlayerActionStateMachine machine) {
            machine.ExitADS();
            isAiming.Value = false;
        }
    }
}