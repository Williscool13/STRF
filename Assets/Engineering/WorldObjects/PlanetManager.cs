using KinematicCharacterController;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace TeleporterSystem
{
    public class PlanetManager : MonoBehaviour, IMoverController
    {
        public PhysicsMover PlanetMover;
        [Title("Gravity")]
        [SerializeField] float Range = 10;
        [SerializeField] float Strength = 10;
        [Title("Orbit")]
        [SerializeField] Vector3 OrbitAxis = Vector3.forward;
        [SerializeField] float OrbitSpeed = 10;

        Dictionary<IGravity, Vector3> storedGravity = new();
        private Quaternion _lastRotation;

        [SerializeField] TeleporterManager teleportManager;

        void Start() {
            teleportManager.GetSource().OnCharacterTeleport += OnTeleportToPlanet;
            teleportManager.GetDestination().OnCharacterTeleport += OnTeleportToGround;

            _lastRotation = PlanetMover.transform.rotation;

            PlanetMover.MoverController = this;
        }

        private void Update() {
            // if outside of range, remove gravity
        }

        void OnTeleportToPlanet(object o, ITeleportable target) {
            Debug.Log("Teleporting to planet");
            IGravity igrav = target as IGravity;
            if (igrav == null) { return; }
            if (storedGravity.ContainsKey(igrav)) { return; }

            storedGravity.Add(igrav, igrav.Gravity);
        }
        void OnTeleportToGround(object o, ITeleportable target) {
            Debug.Log("Teleporting to ground");
            IGravity igrav = target as IGravity;
            if (igrav == null) { return; }
            if (!storedGravity.ContainsKey(target as IGravity)) { return; }

            igrav.Gravity = storedGravity[igrav];
            storedGravity.Remove(target as IGravity);
        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime) {
            goalPosition = PlanetMover.Rigidbody.position;

            // Rotate
            Quaternion targetRotation = Quaternion.Euler(OrbitAxis * OrbitSpeed * deltaTime) * _lastRotation;
            goalRotation = targetRotation;
            _lastRotation = targetRotation;

            foreach (IGravity igrav in storedGravity.Keys) {
                igrav.Gravity = (PlanetMover.transform.position - igrav.Position).normalized * Strength;
            }
        }
    }


    public interface IGravity
    {
        Vector3 Gravity { get; set; }
        Vector3 Position { get; }
    }
}