using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Sprint/Update")]
    public class SprintUpdate : PlayerMovementStateAction
    {
        public override void Execute(PlayerMovementStateMachine machine) {
        }
    }
}