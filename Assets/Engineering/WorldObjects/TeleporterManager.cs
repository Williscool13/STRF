using ScriptableObjectDependencyInjection;
using UnityEngine;

namespace TeleporterSystem
{
    public class TeleporterManager : MonoBehaviour
    {
        [SerializeField] private bool oneWay = false;
        [SerializeField] private TeleportType teleportType;
        [SerializeField] Teleporter source;
        [SerializeField] Teleporter destination;
        [SerializeField] GameObjectEvent OnTeleportEvent;
        //[SerializeField] IntegerVariable teleportedCount;
        private void Start() {
            source.Initialize(this, destination, teleportType, true);
            destination.Initialize(this, source, teleportType, !oneWay);
            source.OnCharacterTeleport += OnTeleported;
        }

        void OnTeleported(object o, ITeleportable target) {
            if (target is MonoBehaviour mono) {
                OnTeleportEvent.Raise(mono.gameObject);
            }
        }


        

        public Teleporter GetSource() => source;
        public Teleporter GetDestination() => destination;
    }

    public enum TeleportType
    {
        Position,
        Rotation,
        PositionAndRotation
    }
    public interface ITeleportable
    {
        public void Teleport(Vector3 position, Quaternion rotation);
        public void Teleport(Vector3 position);
        public void Teleport(Quaternion rotation);
    }
    public interface ITeleporter
    {
        public void Initialize(TeleporterManager manager, Teleporter sister, TeleportType teleportType, bool teleportActive);
        public void IsBeingTeleportedTo();
    }
}