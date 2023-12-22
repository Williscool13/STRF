using System;
using UnityEngine;

namespace TeleporterSystem
{
    public class Teleporter : MonoBehaviour, ITeleporter
    {
        TeleportType teleportType;
        Teleporter destination;
        bool isBeingTeleportedTo = false;

        public event EventHandler<ITeleportable> OnCharacterTeleport;
        public void Initialize(TeleporterManager manager, Teleporter sister, TeleportType teleportType, bool teleportActive) {
            if (!teleportActive) { gameObject.SetActive(false); }
            this.teleportType = teleportType;
            this.destination = sister;
        }

        public void IsBeingTeleportedTo() => isBeingTeleportedTo = true;

        private void OnTriggerEnter(Collider other) {
            if (isBeingTeleportedTo) { isBeingTeleportedTo = false; return; }

            ITeleportable target = other.gameObject.GetComponent<ITeleportable>();
            if (target != null) {
                OnCharacterTeleport?.Invoke(this, target);
                destination.IsBeingTeleportedTo();

                switch (teleportType) {
                    case TeleportType.Position:
                        target.Teleport(destination.transform.position);
                        break;
                    case TeleportType.Rotation:
                        target.Teleport(destination.transform.rotation);
                        break;
                    case TeleportType.PositionAndRotation:
                        target.Teleport(destination.transform.position, destination.transform.rotation);
                        break;
                }
            }
        }
    }
}