using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(fileName = "IdleShoot", menuName = "Finite State Machine/Player Action/Actions/Idle/Idle Shoot")]
    public class IdleShoot : PlayerActionStateAction
    {
        public override void Execute(PlayerActionStateMachine machine) {
            
        }
    }
}