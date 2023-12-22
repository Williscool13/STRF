using FiniteStateMachine;
using KinematicCharacterController;
using ScriptableObjectDependencyInjection;
using System;
using TeleporterSystem;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    public class PlayerMovementStateMachine : BaseStateMachine, ICharacterController, ITeleportable, IGravity, IKnockable, IFace, IVacuumable
    {
        [SerializeField] private KinematicCharacterMotor motor;


        [SerializeField] private PlayerMovementState _initialState;
        [SerializeField] private PlayerAimController aimController;
        //[SerializeField] private GunRigController gunRigController;
        [SerializeField] private Animator animator;

        [Header("Adjacent State Machines")]
        [SerializeField] private PlayerActionStateMachine playerActionStateMachine;

        [Header("Air Movement")]
        [SerializeField] float AirAccelerationSpeed = 10.0f;
        [SerializeField] float AirMaxSpeed = 10.0f;
        [SerializeField] float AirDrag = 0.1f;

        [Header("Jumping")]
        [SerializeField] private bool allowJumpingWhenSliding = false;
        [SerializeField] private float jumpUpSpeed = 10f;
        // bunny hopping - not currently working, look into later
        [SerializeField] private float jumpPreGroundingGraceTime = 0f;
        // coyote time
        [SerializeField] private float jumpPostGroundingGraceTime = 0f;

        [Header("Crouching")]
        [SerializeField] private float CrouchedCapsuleHeight = 1f;

        [Header("Collision")]
        [SerializeField] private LayerMask ignoredCollisionLayers;

        [Header("Misc")]
        [SerializeField] FloatReference playerScale;
        [SerializeField] float GravityOrientationSharpness = 10.0f;

        [SerializeField] private Vector3 gravity = new Vector3(0, -30f, 0);

        // IGravity Properties
        public Vector3 Gravity { get { return gravity; } set { gravity = value; } }
        public Vector3 Position => transform.position;

        [Header("Footsteps")]
        [SerializeField] private FootstepManager playerFootstepManager;

        [Sirenix.OdinInspector.ReadOnly][SerializeField] Vector3 currentLocalVelocity = Vector3.zero;


        public event EventHandler<PlayerMovementStateChangeEventArgs> OnPlayerMovementStateChange;

        public PlayerMovementInputs Inputs { get { return inputs; } }
        private PlayerMovementInputs inputs = new();
        public Vector3 CurrentLocalVelocity => currentLocalVelocity;
        Vector3 PlanarDirection { get; set; }
        public PlayerAimController AimController => aimController;

        /// State Machine Properties
        public PlayerActionStateMachine PlayerActionStateMachine => playerActionStateMachine;
        public PlayerMovementState CurrentState { get; set; }

        public float StateMaxSpeed { get; set; } = 1f;
        public float StateAccelUpSpeed { get; set; } = 1f;
        public float StateAccelDownSpeed { get; set; } = 1f;

        public bool JumpRequested { get; set; }
        public float TimeSinceJumpRequested { get; set; } = float.MaxValue;
        public float JumpForwardMultiplier { get; set; } = 1f;


        /// Jump Properties
        // coyote time
        public KinematicCharacterMotor Motor => motor;


        private float _timeSinceLastAbleToJump = 0f;
        bool _jumpedThisFrame;
        bool _jumpConsumed;

        bool midAir = false;
        Vector3 _internalVelocityAdd = Vector3.zero;


        public override void Awake() {
            CurrentState = _initialState;
            CurrentState.Enter(this);
        }

        private void Start() {

            Motor.CharacterController = this;
            PlanarDirection = transform.forward;
        }

        public override void Update() {
            CurrentState.Execute(this);
            Vector2 recoil = aimController.ParseRecoil(Time.deltaTime);
            recoilXDeltaSoFar += recoil.x;
            //Rotate(recoil.x * Time.deltaTime);
            aimController.RotateVertical(inputs.planarRot, inputs.RawLook.y, recoil.y, Time.deltaTime);
            /*if (inputs.CrouchPress) {
                animator.SetBool("Slide", !animator.GetBool("Slide"));
            }*/

            animator.SetFloat("MovementSpeedMultiplier", 1/playerScale.Value);
        }



        public void TransitionState(PlayerMovementState targetState) {
            PlayerMovementStateChangeEventArgs args = new() {
                PreviousState = CurrentState,
                NewState = targetState
            };
            CurrentState.Exit(this);
            CurrentState = targetState;
            CurrentState.Enter(this);
            OnPlayerMovementStateChange?.Invoke(this, args);
        }



        #region Animation
        public void SetAnimatorBool(string name, bool value) {
            animator.SetBool(name, value);
        }
        public void SetAnimatorFloat(string name, float value) {
            animator.SetFloat(name, value);
        }
        public void SetAnimatorTrigger(string name) {
            animator.SetTrigger(name);
        }
        #endregion

        private Collider[] _probedColliders = new Collider[8];

        public void MotorCrouch() {
            Motor.SetCapsuleDimensions(0.5f, CrouchedCapsuleHeight, CrouchedCapsuleHeight * 0.5f);
        }
        public bool TryUncrouch() {

            Motor.SetCapsuleDimensions(0.5f, 2f, 1f);
            if (Motor.CharacterOverlap(
                Motor.TransientPosition,
                Motor.TransientRotation,
                _probedColliders,
                Motor.CollidableLayers,
                QueryTriggerInteraction.Ignore) > 0) {
                // If obstructions, just stick to crouching dimensions
                Motor.SetCapsuleDimensions(0.5f, CrouchedCapsuleHeight, CrouchedCapsuleHeight * 0.5f);
                return false;
            }
            else {
                // If no obstructions, uncrouch
                return true;
            }
        }
        float recoilXDeltaSoFar = 0f;
        public void SetInput(Vector2 move, Vector2 look, bool sprintPress, bool sprintHold, bool crouchPress, bool crouchHold, bool jumpPress, bool jumpHold) {
            Vector3 moveInputVector = new Vector3(move.x, 0f, move.y);

            // Calculate camera direction and rotation on the character plane
            Quaternion transformRotation = transform.rotation;
            Vector3 transformPlanarDirection = Vector3.ProjectOnPlane(transformRotation * Vector3.forward, Motor.CharacterUp).normalized;
            /*if (transformPlanarDirection.sqrMagnitude == 0f) {
                transformPlanarDirection = Vector3.ProjectOnPlane(transformRotation * Vector3.up, Motor.CharacterUp).normalized;
            }*/
            Quaternion cameraPlanarRotation = Quaternion.LookRotation(transformPlanarDirection, Motor.CharacterUp);

            this.inputs.Move = cameraPlanarRotation * moveInputVector;


            // Convert the world-relative move input vector into a local-relative
            Quaternion rotationFromInput = Quaternion.Euler(transform.up * (AimController.GetXRotateDelta(look.x) + recoilXDeltaSoFar));
            recoilXDeltaSoFar = 0f;
            PlanarDirection = rotationFromInput * PlanarDirection;
            PlanarDirection = Vector3.Cross(transform.up, Vector3.Cross(PlanarDirection, transform.up));
            Quaternion planarRot = Quaternion.LookRotation(PlanarDirection, transform.up);

            // planar (horizontal) rotation
            this.inputs.planarRot = planarRot;
            // planar (horizontal) direction
            this.inputs.planarDir = PlanarDirection;

            this.inputs.RawMove = move;
            this.inputs.RawLook = look;
            this.inputs.SprintPress = sprintPress;
            this.inputs.SprintHold = sprintHold;
            this.inputs.CrouchPress = crouchPress;
            this.inputs.CrouchHold = crouchHold;
            this.inputs.JumpPress = jumpPress;
            this.inputs.JumpHold = jumpHold;
        }

        #region ITeleportable
        public void Teleport(Vector3 position) {
            motor.SetPosition(position);
        }
        public void Teleport(Quaternion rotation) {
            motor.SetRotation(rotation);
            PlanarDirection = transform.forward;
        }
        public void Teleport(Vector3 position, Quaternion rotation) {
            Motor.SetPositionAndRotation(position, rotation);
            PlanarDirection = transform.forward;
        }
        #endregion

        #region ICharacterController
        public void BeforeCharacterUpdate(float deltaTime) {
        }

            
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
            currentRotation = Quaternion.LookRotation(inputs.planarDir, Motor.CharacterUp);

            Vector3 currentUp = (currentRotation * Vector3.up);


            // Rotates the character to align with gravity
            Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, -Gravity.normalized, 1 - Mathf.Exp(-GravityOrientationSharpness * deltaTime));
            currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;

        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            if (Motor.GroundingStatus.IsStableOnGround) {
                float currentVelocityMagnitude = currentVelocity.magnitude;

                Vector3 effectiveGroundNormal = Motor.GroundingStatus.GroundNormal;

                // Reorient velocity on slope
                currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

                // Calculate target velocity
                Vector3 inputRight = Vector3.Cross(inputs.Move, Motor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * inputs.Move.magnitude;
                Vector3 targetMovementVelocity = reorientedInput * StateMaxSpeed;

                // Smooth movement Velocity
                LerpMovementSpeed(ref currentVelocity, targetMovementVelocity, inputs.RawMove, StateMaxSpeed, StateAccelUpSpeed, StateAccelDownSpeed);
                

                this.currentLocalVelocity = transform.InverseTransformDirection(currentVelocity);
                animator.SetBool("Jump", false);
                SetAnimatorFloat("MovementX", Mathf.MoveTowards(animator.GetFloat("MovementX"), StateMaxSpeed == 0 ? 0 : inputs.RawMove.x, deltaTime / 0.15f));
                SetAnimatorFloat("MovementY", Mathf.MoveTowards(animator.GetFloat("MovementY"), StateMaxSpeed == 0 ? 0 : inputs.RawMove.y, deltaTime / 0.15f));

                if (midAir) {
                    midAir = false;
                    playerFootstepManager.PlayLandSound();
                }
            }
            // Air movement
            else {
                // Add move input
                if (inputs.Move.sqrMagnitude > 0f) {
                    Vector3 addedVelocity = inputs.Move * AirAccelerationSpeed * deltaTime;

                    Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, Motor.CharacterUp);

                    // Limit air velocity from inputs
                    if (currentVelocityOnInputsPlane.magnitude < AirMaxSpeed) {
                        // clamp addedVel to make total vel not exceed max vel on inputs plane
                        Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, AirMaxSpeed);
                        addedVelocity = newTotal - currentVelocityOnInputsPlane;
                    }
                    else {
                        // Make sure added vel doesn't go in the direction of the already-exceeding velocity
                        if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f) {
                            addedVelocity = Vector3.ProjectOnPlane(addedVelocity, currentVelocityOnInputsPlane.normalized);
                        }
                    }

                    // Prevent air-climbing sloped walls
                    if (Motor.GroundingStatus.FoundAnyGround) {
                        if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f) {
                            Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                            addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpenticularObstructionNormal);
                        }
                    }

                    // Apply added velocity
                    currentVelocity += addedVelocity;
                }

                // Gravity
                currentVelocity += gravity * deltaTime;

                // Drag
                currentVelocity *= (1f / (1f + (AirDrag * deltaTime)));

                midAir = true;
            }

            HandleJumping(ref currentVelocity, deltaTime, inputs.Move);


            // Take into account additive velocity
            if (_internalVelocityAdd.sqrMagnitude > 0f) {
                currentVelocity += _internalVelocityAdd;
                _internalVelocityAdd = Vector3.zero;
                Motor.ForceUnground();
            }
        }


        public void AfterCharacterUpdate(float deltaTime) {
            // Handle jump-related values
            {
                // Handle jumping pre-ground grace period (Bunny Hopping)
                if (JumpRequested && TimeSinceJumpRequested > jumpPreGroundingGraceTime) {
                    JumpRequested = false;
                }

                if (allowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround) {
                    // If we're on a ground surface, reset jumping values
                    if (!_jumpedThisFrame) {
                        _jumpConsumed = false;
                    }
                    _timeSinceLastAbleToJump = 0f;
                }
                else {
                    // Keep track of time since we were last able to jump (for grace period)
                    _timeSinceLastAbleToJump += deltaTime;
                }
            }

        }

        public void PostGroundingUpdate(float deltaTime) {
        }


        public bool IsColliderValidForCollisions(Collider coll) {
            if (ignoredCollisionLayers == (ignoredCollisionLayers | (1 << coll.gameObject.layer))) {
                return false;
            }
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) {
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) {
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) {
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider) {
        }

        #endregion

        public bool CanJump() {
            return IsGrounded() || _timeSinceLastAbleToJump <= jumpPostGroundingGraceTime;
        }

        public bool IsGrounded() {
            return (allowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround);
        }

        public bool IsForceUngrounded() {
            return Motor.MustUnground();
        }

        void HandleJumping(ref Vector3 currentVelocity, float deltaTime, Vector3 moveInputVector) {
            // Handle jumping
            _jumpedThisFrame = false;
            TimeSinceJumpRequested += deltaTime;
            if (!JumpRequested) { return; }

            // See if we actually are allowed to jump
            if (!_jumpConsumed) { 
                // Calculate jump direction before ungrounding
                Vector3 jumpDirection = Motor.CharacterUp;
                if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround) {
                    jumpDirection = Motor.GroundingStatus.GroundNormal;
                }

                // Makes the character skip ground probing/snapping on its next update. 
                // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                Motor.ForceUnground();

                // Add to the return velocity and reset jump state
                currentVelocity += (jumpDirection * jumpUpSpeed * playerScale.Value) - Vector3.Project(currentVelocity, Motor.CharacterUp);
                currentVelocity += (moveInputVector * JumpForwardMultiplier);
                JumpRequested = false;
                _jumpConsumed = true;
                _jumpedThisFrame = true;


                animator.SetBool("Jump", true);

                // footstep manager play jump sound
                playerFootstepManager.PlayJumpSound();
            }
        }



        void LerpMovementSpeed(ref Vector3 currentVelocity, Vector3 targetMovementVelocity, Vector2 rawMovementInput, float maxSpeed, float stateLerpUpSpeed, float stateLerpDownSpeed) {
            if (rawMovementInput.magnitude > 0 && currentVelocity.magnitude < targetMovementVelocity.magnitude) {
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, stateLerpUpSpeed * Time.deltaTime);
            } else {
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, stateLerpDownSpeed * Time.deltaTime);
            }
        }

        public Vector3 GetFacingDirection() {
            Vector3 right = Vector3.Cross(Vector3.up, PlanarDirection).normalized;
            Quaternion targetRot = Quaternion.AngleAxis(aimController.GetXFacingAngle(), right);
            return targetRot * PlanarDirection;
        }

        public void AddForce(Vector3 point, Vector3 normalizedForceVector, float sourceScale, float forceStrength) {
            _internalVelocityAdd += (normalizedForceVector * forceStrength) * (1 / playerScale.Value);
        }

        public void Vacuum(Vector3 point, Vector3 normalizedForceVector, float sourceScale, float forceStrength) {
            AddForce(point, normalizedForceVector, sourceScale, forceStrength * 1 / playerScale.Value);
        }
        bool sprinting = false;
        public void SetSprint(bool val) {
            sprinting = val;
            aimController.Sprinting = val;
        }
    }

    public class PlayerMovementStateChangeEventArgs : EventArgs {
        public PlayerMovementState PreviousState { get; set; }
        public PlayerMovementState NewState { get; set; }
    }

}

public struct PlayerMovementInputs
{
    public bool CrouchPress;
    public bool CrouchHold;
    public bool SprintPress;
    public bool SprintHold;
    public bool JumpPress;
    public bool JumpHold;
    public Vector3 Move;
    public Quaternion planarRot;
    public Quaternion verticalLook;

    public Vector2 RawMove;
    public Vector2 RawLook;

    public Vector3 planarDir;

}

public interface IKnockable
{
    public void AddForce(Vector3 point, Vector3 normalizedForceDirection, float sourceScale, float forceStrength);
}

public static class TransformExtensions
{
    public static float GetScaleAverage(this Transform transform) {
        Vector3 localScale = transform.localScale;
        return (localScale.x + localScale.y + localScale.z) / 3f;
    }
}

public static class Vector3Extensions
{
    public static float GetVectorValuesAverage(this Vector3 vector) {
        return (vector.x + vector.y + vector.z) / 3f;
    }
}

public interface IFace
{
    public Vector3 GetFacingDirection();
}