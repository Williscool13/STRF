using System.Collections;
using System.Collections.Generic;
using TeleporterSystem;
using UnityEngine;

public class BallTeleportable : MonoBehaviour, ITeleportable
{
    public void Teleport(Vector3 position, Quaternion rotation) {
        transform.position = position;
        transform.rotation = rotation;
    }

    public void Teleport(Vector3 position) {
        transform.position = position;
    }

    public void Teleport(Quaternion rotation) {
        transform.rotation = rotation;
    }
}
