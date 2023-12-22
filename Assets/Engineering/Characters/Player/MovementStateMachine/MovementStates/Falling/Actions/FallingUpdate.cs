using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Falling/Update")]
    public class FallingUpdate : PlayerMovementStateAction {
        public override void Execute(PlayerMovementStateMachine machine) {
            // totalFallTime += Time.deltaTime;
        }

    }

}