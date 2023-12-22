using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Sprint/Enter")]
    public class SprintEnter : PlayerMovementStateAction {
        [SerializeField] BoolVariable isSprinting;

        [SerializeField] float maxSpeed = 12f;
        [SerializeField] float accelUpSpeed = 12f;
        [SerializeField] float accelDownSpeed = 20f;

        public override void Execute(PlayerMovementStateMachine machine) {
            machine.SetAnimatorBool("Sprint", true);
            isSprinting.Value = true;
            machine.SetSprint(true);
            machine.StateMaxSpeed = maxSpeed;
            machine.StateAccelUpSpeed = accelUpSpeed;
            machine.StateAccelDownSpeed = accelDownSpeed;
        }
    }
}
