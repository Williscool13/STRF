using DG.Tweening;
using FiniteStateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerFiniteStateMachine
{
    public class PlayerActionStateMachine : BaseStateMachine, IWeaponLoadout
    {
        [SerializeField] private PlayerActionState _initialState;
        [SerializeField] private PlayerMovementStateMachine playerMovementStateMachine;
        [SerializeField] Animator animator;

        //[SerializeField] private GunRigController gunRigController;
        [SerializeField] private PlayerLoadoutManager playerLoadoutManager;
        [SerializeField] private PlayerAimController aimController;
        public event EventHandler<PlayerActionStateChangeEventArgs> OnPlayerActionStateChange;

        public PlayerMovementStateMachine PlayerMovementStateMachine => playerMovementStateMachine;
        public PlayerActionState CurrentState { get; set; }
        //public GunRigController GunRigController { get { return gunRigController; } }
        public PlayerLoadoutManager PlayerLoadoutManager { get { return playerLoadoutManager; } }
        public PlayerAimController PlayerAimController { get { return aimController; } }

        

        public PlayerActionInputs Inputs { get { return inputs; } }
        PlayerActionInputs inputs = new PlayerActionInputs();

        public override void Awake() {
            CurrentState = _initialState;
            CurrentState.Enter(this);
            DOTween.Init()
                .SetCapacity(400, 300);
        }

        private void Start() {
        }

        public override void Update() {

            CurrentState.Execute(this);
        }


        public void TransitionState(PlayerActionState targetState) {
            PlayerActionStateChangeEventArgs args = new() { 
                PreviousState = CurrentState, 
                NewState = targetState 
            };
            CurrentState.Exit(this);
            CurrentState = targetState;
            CurrentState.Enter(this);
            OnPlayerActionStateChange?.Invoke(this, args);
        }

        public void SetAnimatorTrigger(string name) {
            animator.SetTrigger(name);
        }
        public void SetAnimatorFloat(string name, float value) {
            animator.SetFloat(name, value);
        }
        public void SetAnimatorBool(string name, bool value) {
            animator.SetBool(name, value);
        }

        public void EnterADS() {
            SetAnimatorBool("Aim", true);
            PlayerAimController.AimingDownSights = true;
            PlayerLoadoutManager.GetCurrentWeapon().AimStart();

        }
        public void ExitADS() {
            SetAnimatorBool("Aim", false);
            PlayerAimController.AimingDownSights = false;
            PlayerLoadoutManager.GetCurrentWeapon().AimEnd();
        }

        public void ReloadWeaponMovementFPS(float reloadTime) {
            PlayerLoadoutManager.ReloadMovement(reloadTime);
        }

        public void SwapWeaponMovementFPS(float swapTime) {
            PlayerLoadoutManager.SwapMovement(swapTime);
        }

        public void SwapWeaponTransforms() {
            PlayerLoadoutManager.SwapWeaponTransforms();
        }

        public void SetInputs(bool reloadPress, bool reloadHold, bool swapPress, bool swapHold, bool shootPress, bool shootHold, bool aimPress, bool aimHold) {
            this.inputs.SwapPress = swapPress;
            this.inputs.SwapHold = swapHold;
            this.inputs.ReloadPress = reloadPress;
            this.inputs.ReloadHold = reloadHold;
            this.inputs.ShootPress = shootPress;
            this.inputs.ShootHold = shootHold;
            this.inputs.AimPress = aimPress;
            this.inputs.AimHold = aimHold;
        }

        public void IncreaseMouseSensitivity() {
            aimController.IncreaseSensitivity();
        }
        public void DecreaseMouseSensitivity() {
            aimController.DecreaseSensitivity();
        }
    }

    public class PlayerActionStateChangeEventArgs : EventArgs
    {
        public PlayerActionState PreviousState { get; set; }
        public PlayerActionState NewState { get; set; }
    }
}

public struct PlayerActionInputs
{
    public bool ReloadPress;
    public bool ReloadHold;
    public bool SwapPress;
    public bool SwapHold;
    public bool ShootPress;
    public bool ShootHold;
    public bool AimPress;
    public bool AimHold;
}

public interface IWeaponLoadout
{
    public void ReloadWeaponMovementFPS(float reloadTime);
    public void SwapWeaponMovementFPS(float swapTime);
    public void SwapWeaponTransforms();
}