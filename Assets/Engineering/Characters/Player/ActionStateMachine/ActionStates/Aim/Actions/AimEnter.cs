using PlayerFiniteStateMachine;
using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(fileName = "AimEnter", menuName = "Finite State Machine/Player Action/Actions/Aim/Aim Enter")]
    public class AimEnter : PlayerActionStateAction {
        [SerializeField] private BoolVariable isAiming;
        public override void Execute(PlayerActionStateMachine machine) {
            machine.EnterADS();
            isAiming.Value = true;
        }
    }
}