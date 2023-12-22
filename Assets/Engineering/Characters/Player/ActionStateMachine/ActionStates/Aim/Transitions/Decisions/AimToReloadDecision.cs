using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Decisions/Aim/Aim To Reload")]
    public class AimToReloadDecision : PlayerActionStateDecision
    {
        public override bool Decide(PlayerActionStateMachine Machine) {
            if (Machine.Inputs.ReloadPress && Machine.PlayerLoadoutManager.GetCurrentWeapon().CanReload()) {
                return true;
            }

            return false;
        }
    }
}