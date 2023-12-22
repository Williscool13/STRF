using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(fileName = "AimUpdate", menuName = "Finite State Machine/Player Action/Actions/Aim/Aim Update")]
    public class AimUpdate : PlayerActionStateAction
    {
        public override void Execute(PlayerActionStateMachine machine) {

        }
    }
}