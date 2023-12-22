using Cinemachine;
using DG.Tweening;
using KinematicCharacterController;
using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerAimController : MonoBehaviour
{

    const float baseCameraDistance = 0.1f;
    const float aimCameraDistance = 0.4f;

    const float baseCrouchHeight = 0.1f;
    const float crouchHeightDiff = -0.4f;


    [SerializeField] private FloatVariable sensitivityMultiplier;
    float currXSens = 6f;
    float currYSens = 8f;

    float XMouseSensitivity => sensitivityMultiplier.Value * currXSens;
    float YMouseSensitivity => sensitivityMultiplier.Value * currYSens;

    public void IncreaseSensitivity() {
        sensitivityMultiplier.Value = Mathf.Clamp(sensitivityMultiplier.Value + 0.05f, 0.01f, 10f);
    }
    public void DecreaseSensitivity() {
        sensitivityMultiplier.Value = Mathf.Clamp(sensitivityMultiplier.Value - 0.05f, 0.01f, 10f);
    }

    [SerializeField] private float xRecoilRecoverySpeed = 20f;
    [SerializeField] private Transform playerModelYRotPivot;
    [SerializeField] private Vector2 playerModelYRotClamps = new Vector2(-50f, 50f);
    [SerializeField] private Transform cameraYRotPivot;
    [SerializeField] private Vector2 cameraYRotClamps = new Vector2(-70f, 70f);


    [SerializeField] private float aimZoomSpeed = 2f;
    [SerializeField] private FloatReference aimRecoilMultiplier;
    [SerializeField] private FloatReference aimSensitivityMultiplier;

    [SerializeField] private float crouchTransitionSpeed = 5f;
    [SerializeField] private FloatReference crouchRecoilMultiplier;
    [SerializeField] private FloatReference crouchSensitivityMultiplier;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Title("Sprint Position")]
    [SerializeField] AimConstraint aimConstraint;
    CinemachineTransposer follow;
    public bool AimingDownSights { get; set; }
    public bool Sprinting { get; set; }
    public bool Crouching { get; set; }

    float _targetVerticalAngle = 0f;

    List<RecoilData> recoils = new();
    private void Start() {
        follow = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        baseZoomOffset = follow.m_FollowOffset;
        baseHeight = follow.m_FollowOffset.y;
    }

    void Update() {
        AimZoom();
        AimSprint();
    }
    float aimZoomCurr = 0;
    Vector3 baseZoomOffset;
    float aimZoomMultiplier = 0.3f;

    float crouchCurr = 0;
    float baseHeight;
    void AimZoom() {
        crouchCurr = Mathf.MoveTowards(crouchCurr, Crouching ? 1 : 0, Time.deltaTime * crouchTransitionSpeed);
        float crouchHeight = baseHeight + crouchHeightDiff * crouchCurr;

        aimZoomCurr = Mathf.MoveTowards(aimZoomCurr, AimingDownSights ? 1 : 0, Time.deltaTime * aimZoomSpeed);
        Vector3 target = baseZoomOffset;
        target.y = crouchHeight;
        Vector3 aimOffset = aimZoomCurr * aimZoomMultiplier * transform.InverseTransformDirection(cameraYRotPivot.forward);
        follow.m_FollowOffset =  target + aimOffset;

    }
    [SerializeField] float rotationOffsetSpeed = 10f;
    [SerializeField] float fieldOfViewSpeed = 10f;
    [SerializeField] float sprintRotationOffset = 75f;
    [SerializeField] float sprintFoV = 65f;
    void AimSprint() {

        Vector3 tar = aimConstraint.rotationOffset;
        tar.x = Mathf.MoveTowards(tar.x, Sprinting? GetXRot() : 0.0f, Time.deltaTime * rotationOffsetSpeed);
        tar.y = Mathf.MoveTowards(tar.y, Sprinting ? sprintRotationOffset : 0.0f, Time.deltaTime * rotationOffsetSpeed);
        aimConstraint.rotationOffset = tar;
        virtualCamera.m_Lens.FieldOfView = Mathf.MoveTowards(virtualCamera.m_Lens.FieldOfView, Sprinting ? sprintFoV : 60.0f, Time.deltaTime * fieldOfViewSpeed);
    }
    [SerializeField] float xRotSinFreq = 10.0f;
    [SerializeField] float xRotSinAmp = 10.0f;
    [SerializeField] float xRotSinOffset = -10.0f;
    float GetXRot() {
        float v = xRotSinOffset +  Mathf.Sin(Time.time * xRotSinFreq) * xRotSinAmp;
        return v;

    }
    /// <summary>
    /// Parse stored recoil data and return the total recoil for this frame
    /// </summary>
    /// <param name="deltaTime"></param>
    /// <returns></returns>
    Vector2 Recoil(float deltaTime) {
        float totalYRecoil = 0;
        float totalXRecoil = 0;
        for (int i = 0; i < recoils.Count; i++) {
            if (recoils[i].duration <= 0) {
                recoils.RemoveAt(i);
            }
        }

        if (recoils.Count > 0) {

            for (int i = 0; i < recoils.Count; i++) {
                totalYRecoil += recoils[i].RecoilKick.y / recoils[i].totalDuration;
                totalXRecoil += recoils[i].RecoilKick.x / recoils[i].totalDuration;

                recoils[i].duration -= deltaTime;
            }

            totalYRecoil /= recoils.Count;
            totalXRecoil /= recoils.Count;

            if (AimingDownSights) {
                totalYRecoil *= aimRecoilMultiplier.Value;
                totalXRecoil *= aimRecoilMultiplier.Value;
            }
            if (Crouching) {
                totalYRecoil *= crouchRecoilMultiplier.Value;
                totalXRecoil *= crouchRecoilMultiplier.Value;
            }
        }

        return new Vector2(totalXRecoil, totalYRecoil);
    }

    public Vector2 ParseRecoil(float deltaTime) {
        return Recoil(deltaTime) * deltaTime;
    }

    public void RotateVertical(Quaternion planarRot, float rawYInput, float yRecoil, float deltaTime) {
        _targetVerticalAngle -= (rawYInput * YMouseSensitivity * deltaTime) + yRecoil;

        _targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle, cameraYRotClamps.x, cameraYRotClamps.y);
        Quaternion verticalRot = Quaternion.Euler(_targetVerticalAngle, cameraYRotPivot.eulerAngles.y, cameraYRotPivot.eulerAngles.z);

        cameraYRotPivot.rotation = planarRot * verticalRot;
        cameraYRotPivot.localEulerAngles = new Vector3(cameraYRotPivot.localEulerAngles.x, 0, 0);

        float _mappedTargetVerticalAngle = MapAngle(_targetVerticalAngle, cameraYRotClamps, playerModelYRotClamps);
        Quaternion playerModelVerticalRot = Quaternion.Euler(_mappedTargetVerticalAngle, playerModelYRotPivot.eulerAngles.y, playerModelYRotPivot.eulerAngles.z);
        playerModelYRotPivot.rotation = planarRot * playerModelVerticalRot;
        playerModelYRotPivot.localEulerAngles = new Vector3(playerModelYRotPivot.localEulerAngles.x, 0, 0);
    }

    public float GetXRotateDelta(float value) {
        return value * Time.deltaTime * XMouseSensitivity * (AimingDownSights ? aimSensitivityMultiplier.Value : 1f);
    }


    float MapAngle(float angle, Vector2 from, Vector2 to) => (angle - from.x) / (from.y - from.x) * (to.y - to.x) + to.x;

    public float GetXFacingAngle() { return cameraYRotPivot.localEulerAngles.x; }

    public void AddRecoil(RecoilData data) => recoils.Add(data);


}
