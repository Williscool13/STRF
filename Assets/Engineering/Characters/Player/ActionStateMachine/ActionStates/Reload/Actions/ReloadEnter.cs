using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Actions/Reload/Enter")]
    public class ReloadEnter : PlayerActionStateAction
    {
        [SerializeField] bool animBasedReloadTime = false;
        [SerializeField] BoolVariable isReloading;
        [SerializeField] FloatVariable reloadTimeleft;
        [ShowIf("animBasedReloadTime")]
        [SerializeField] AnimationClip anim;
        [ShowIf("@!animBasedReloadTime")]
        [SerializeField] FloatReference reloadTime;
        public override void Execute(PlayerActionStateMachine machine) {
            isReloading.Value = true;

            machine.PlayerLoadoutManager.GetCurrentWeapon().ReloadStart();
            
            float reloadSpeedMultiplier = machine.PlayerLoadoutManager.GetCurrentWeapon().ReloadSpeedMultiplier;
            // 0.75 is the hard coded exit time of the reload animation
            if (animBasedReloadTime) {
                reloadTimeleft.Value = (anim.length * 0.75f) / reloadSpeedMultiplier;

            } else {
                reloadTimeleft.Value = reloadTime.Value / reloadSpeedMultiplier;
            }
            //reloadTimeleft.Value = (anim.length * 0.75f) / reloadSpeedMultiplier;

            machine.SetAnimatorFloat("ReloadSpeedMultiplier", reloadSpeedMultiplier);
            machine.SetAnimatorTrigger("Reload");

            machine.ReloadWeaponMovementFPS(reloadTimeleft.Value);
        }
    }
}