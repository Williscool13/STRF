using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Walk/Update")]
    public class WalkUpdate : PlayerMovementStateAction
    {
        public override void Execute(PlayerMovementStateMachine machine) {
        }

    }

}