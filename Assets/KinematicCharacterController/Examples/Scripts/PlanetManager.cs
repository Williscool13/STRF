using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using System;
using PlayerFiniteStateMachine;

namespace KinematicCharacterController.Examples
{
    public class PlanetManager : MonoBehaviour, IMoverController
    {
        public PhysicsMover PlanetMover;
        public SphereCollider GravityField;
        public float GravityStrength = 10;
        public Vector3 OrbitAxis = Vector3.forward;
        public float OrbitSpeed = 10;

        public Teleporter OnPlaygroundTeleportingZone;
        public Teleporter OnPlanetTeleportingZone;

        private List<ExampleCharacterController> _characterControllersOnPlanet = new List<ExampleCharacterController>();
        private List<PlayerMovementStateMachine> playerMovementStateMachines = new List<PlayerMovementStateMachine>();
        private Vector3 _savedGravity;
        private Quaternion _lastRotation;

        private void Start()
        {
            OnPlaygroundTeleportingZone.OnCharacterTeleport -= ControlGravity;
            OnPlaygroundTeleportingZone.OnCharacterTeleport += ControlGravity;

            OnPlaygroundTeleportingZone.OnCharacterTeleportPlayer -= ControlGravity;
            OnPlaygroundTeleportingZone.OnCharacterTeleportPlayer += ControlGravity;




            OnPlanetTeleportingZone.OnCharacterTeleport -= UnControlGravity;
            OnPlanetTeleportingZone.OnCharacterTeleport += UnControlGravity;

            OnPlanetTeleportingZone.OnCharacterTeleportPlayer -= UnControlGravity;
            OnPlanetTeleportingZone.OnCharacterTeleportPlayer += UnControlGravity;

            _lastRotation = PlanetMover.transform.rotation;

            PlanetMover.MoverController = this;
        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            goalPosition = PlanetMover.Rigidbody.position;

            // Rotate
            Quaternion targetRotation = Quaternion.Euler(OrbitAxis * OrbitSpeed * deltaTime) * _lastRotation;
            goalRotation = targetRotation;
            _lastRotation = targetRotation;

            // Apply gravity to characters
            foreach (ExampleCharacterController cc in _characterControllersOnPlanet)
            {
                cc.Gravity = (PlanetMover.transform.position - cc.transform.position).normalized * GravityStrength;
            }

            foreach (PlayerMovementStateMachine pms in playerMovementStateMachines) {
                pms.Gravity = (PlanetMover.transform.position - pms.transform.position).normalized * GravityStrength;
            }
        }

        void ControlGravity(ExampleCharacterController cc)
        {
            _savedGravity = cc.Gravity;
            _characterControllersOnPlanet.Add(cc);
        }

        void UnControlGravity(ExampleCharacterController cc)
        {
            cc.Gravity = _savedGravity;
            _characterControllersOnPlanet.Remove(cc);
        }

        void ControlGravity(PlayerMovementStateMachine pms) {
            Debug.Log("added player");
            _savedGravity = pms.Gravity;
            playerMovementStateMachines.Add(pms);
        }

        void UnControlGravity(PlayerMovementStateMachine pms) {
            Debug.Log("removed player");
            pms.Gravity = _savedGravity;
            playerMovementStateMachines.Remove(pms);
        }
    }
}