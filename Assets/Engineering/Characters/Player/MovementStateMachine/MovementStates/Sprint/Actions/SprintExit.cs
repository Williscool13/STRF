using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Sprint/Exit")]
    public class SprintExit : PlayerMovementStateAction {
        [SerializeField] BoolVariable isSprinting;
        public override void Execute(PlayerMovementStateMachine machine) {
            machine.SetAnimatorBool("Sprint", false);
            isSprinting.Value = false;
            machine.SetSprint(false);
        }
    }

}