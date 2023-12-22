using PlayerFiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Crouch/Enter")]
    public class CrouchEnter : PlayerMovementStateAction
    {
        [SerializeField] float maxSpeed = 12f;
        [SerializeField] float accelUpSpeed = 12f;
        [SerializeField] float accelDownSpeed = 20f;
        public override void Execute(PlayerMovementStateMachine machine) {
            machine.MotorCrouch();

            // set camera position to crouch
            machine.AimController.Crouching = true;

            machine.SetAnimatorBool("Crouch", true);
            machine.StateMaxSpeed = maxSpeed;
            machine.StateAccelUpSpeed = accelUpSpeed;
            machine.StateAccelDownSpeed = accelDownSpeed;
        }

    }
}