using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Walk/Enter")]
    public class WalkEnter : PlayerMovementStateAction {
        [SerializeField] float maxSpeed = 12f;
        [SerializeField] float accelUpSpeed = 12f;
        [SerializeField] float accelDownSpeed = 20f;
        public override void Execute(PlayerMovementStateMachine machine) {
            machine.StateMaxSpeed = maxSpeed;
            machine.StateAccelUpSpeed = accelUpSpeed;
            machine.StateAccelDownSpeed = accelDownSpeed;
        }

    }

}