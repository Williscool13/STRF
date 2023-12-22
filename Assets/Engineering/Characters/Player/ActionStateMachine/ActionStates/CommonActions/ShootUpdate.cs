using UnityEngine;
using System.Linq;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(fileName = "ShootUpdate", menuName = "Finite State Machine/Player Action/Actions/Common/Shoot Update")]
    public class ShootUpdate : PlayerActionStateAction
    {
        [SerializeField] PlayerMovementState[] whiteList;
        public override void Execute(PlayerActionStateMachine machine) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // if rig is in position
            //if (!machine.GunRigController.RigInPosition()) { return; }
            // check if gun can shoot
            WeaponBase currentWeapon = machine.PlayerLoadoutManager.GetCurrentWeapon();
            if (currentWeapon == null) { return; }

            bool canFire =
                (machine.Inputs.ShootPress && currentWeapon.CanFire(false))
                || (machine.Inputs.ShootHold && currentWeapon.CanFire(true));
            canFire &= whiteList.Contains(machine.PlayerMovementStateMachine.CurrentState);

            if (!canFire) { return; }



            // call gun fire
            RecoilData data = currentWeapon.Fire();
            // run recoil 
            machine.PlayerAimController.AddRecoil(data);

            //machine.PlayerMovementStateMachine.FacingDirectionKnockback(-10f);
            //machine.PlayerMovementStateMachine.AddForce(new Vector3(1, 1, 1));
        }
    }
}