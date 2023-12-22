using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, IMoverController
{
    [SerializeField] PhysicsMover mover;

    [SerializeField] Vector3 TranslationAxis = Vector3.right;
    [SerializeField] float TranslationPeriod = 10;
    [SerializeField] float TranslationSpeed = 1;
    [SerializeField] Vector3 RotationAxis = Vector3.up;
    [SerializeField] float RotSpeed = 10;
    [SerializeField] Vector3 OscillationAxis = Vector3.zero;
    [SerializeField] float OscillationPeriod = 10;
    [SerializeField] float OscillationSpeed = 10;

    private Vector3 _originalPosition;
    private Quaternion _originalRotation;

    private void Start() {
        _originalPosition = mover.Rigidbody.position;
        _originalRotation = mover.Rigidbody.rotation;

        mover.MoverController = this;
    }

    public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime) {
        goalPosition = (_originalPosition + (TranslationAxis.normalized * Mathf.Sin(Time.time * TranslationSpeed) * TranslationPeriod));

        Quaternion targetRotForOscillation = Quaternion.Euler(OscillationAxis.normalized * (Mathf.Sin(Time.time * OscillationSpeed) * OscillationPeriod)) * _originalRotation;
        goalRotation = Quaternion.Euler(RotationAxis * RotSpeed * Time.time) * targetRotForOscillation;
    }
}
