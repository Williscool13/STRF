using FiniteStateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/State")]
    public class PlayerActionState : BaseState<PlayerActionStateMachine>
    {
        //[SerializeField] private GunIKTargets gunIkTarget;
        //public GunIKTargets GunIkTargets => gunIkTarget;
        public List<PlayerActionStateAction> EnterActions = new List<PlayerActionStateAction>();
        public List<PlayerActionStateAction> UpdateActions = new List<PlayerActionStateAction>();
        public List<PlayerActionStateAction> ExitActions = new List<PlayerActionStateAction>();

        public List<PlayerActionStateTransition> Transitions = new List<PlayerActionStateTransition>();

        public override void Execute(PlayerActionStateMachine machine) {
            foreach (PlayerActionStateAction action in UpdateActions)
                action.Execute(machine);

            foreach (PlayerActionStateTransition transition in Transitions) {
                transition.Execute(machine);
                if (machine.CurrentState != this)
                    break;
            }
        }

        public override void Enter(PlayerActionStateMachine machine) {
            foreach (PlayerActionStateAction action in EnterActions)
                action.Execute(machine);
        }

        public override void Exit(PlayerActionStateMachine machine) {
            foreach (PlayerActionStateAction action in ExitActions)
                action.Execute(machine);
        }
    }
}
