using DG.Tweening;
using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour, IKnockable, IVacuumable
{
    [Title("AgentComponents")]
    [SerializeField] Transform agentEyes;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] EnemyHealthSystem healthSystem;
    [SerializeField] SentryGunController gunController;

    

    [Title("Line of Sight")]
    [SerializeField] float lineOfSightRange = 20f;
    [SerializeField] LayerMask lineOfSightLayers;
    [SerializeField] FloatReference playerScale;
    [ReadOnly][SerializeField] Transform target;

    [Title("Ragdoll")]
    [SerializeField] GameObject robotModel;
    [SerializeField] GameObject ragdoll;
    [SerializeField] GameObject ragdollCenterMass;
    [SerializeField] float ragdollForceMultiplier = 20.0f;
    [SerializeField] float despawnTime = 3.0f;

    [Title("Shield")]
    [SerializeField] bool hasShield = false;
    [ShowIf("hasShield")]
    [SerializeField] Animator shieldAnim;

    [Title("Pathfinding")]
    [SerializeField] bool moveTowardsPlayer = true;

    [Title("Vacuum")]
    [DetailedInfoBox("If false, rigidbody will be destroyed. Vacuumable enemies behave poorly with player contact",
        "When the player collides with the robot, the robot will be pushed. (because of the rigidbody)")]
    [SerializeField] bool vacuumable = false;
    IScalable scalable;

    Sequence playerPollSequence;
    void Start()
    {
        playerPollSequence = DOTween.Sequence()
            .AppendCallback(() => PollForPlayer())
            .AppendInterval(2.0f)
            .SetLoops(-1)
            .Play();

        healthSystem.OnEnemyHit += (sender, e) => { };
        healthSystem.OnEnemyDeath += OnEnemyDeath;

        scalable = GetComponent<IScalable>();

        Rigidbody rb = GetComponent<Rigidbody>();
        if (!vacuumable) { Destroy(rb); }
        else {
            if (!moveTowardsPlayer) { rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX; }
        }
    }

 

    void OnEnemyDeath(object sender, HitDataContainer e) {
        if (agent.isOnNavMesh) { agent.isStopped = true; }
        
        state = RobotState.Dead;
        robotModel.SetActive(false);
        gunController.gameObject.SetActive(false);
        ragdoll.SetActive(true);
        playerPollSequence.Kill();

        agent.enabled = false;
        DOTween.Sequence()
            .AppendInterval(despawnTime)
            .OnComplete(() => Destroy(this.gameObject, 0.5f))
            .Play();
    }

    
    enum RobotState {
        Chase,
        Shoot,
        Idle,
        Dead
    }
    RobotState state = RobotState.Idle;
    void Update()
    {
        if (transform.position.y <= -200f) {
            playerPollSequence.Kill();
            Destroy(this.gameObject);
            return;
        }
        bool inRange = PlayerNearby();
        if (target == null || !inRange) {
            anim.SetBool("Walking", false);
            state = RobotState.Idle;
            if (agent.isOnNavMesh) agent.isStopped = true;
            return;
        }

        bool targetInLos = CheckPlayerLineOfSight();

        switch (state) {
            case RobotState.Chase:
                anim.SetBool("Walking", true);
                if (shieldAnim != null) shieldAnim.SetBool("Walking", true);
                
                if (targetInLos) {
                    EnterShoot();
                }
                else {
                    if (AgentAtDestination() || agent.isStopped) {
                        FindAndMoveToPlayer();
                    }
                }

                break;
            case RobotState.Shoot:
                anim.SetBool("Walking", false);
                if (shieldAnim != null) shieldAnim.SetBool("Walking", false);
                
                if (targetInLos) {
                    transform.LookAt(target);
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                }
                else if (moveTowardsPlayer) {
                    EnterChase();
                    
                }
                break;
            case RobotState.Idle:
                anim.SetBool("Walking", false);
                if (shieldAnim != null) shieldAnim.SetBool("Walking", false);
                
                if (target != null) { 
                    if (moveTowardsPlayer) {
                        EnterChase();
                    } else {
                        EnterShoot();
                    }
                }
                break;
            case RobotState.Dead:
                break;
        }
    }

    void EnterChase() {
        state = RobotState.Chase;
        FindAndMoveToPlayer();
    }

    void EnterShoot() {
        if (agent.isOnNavMesh) agent.isStopped = true;
        //transform.LookAt(target);
        //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        state = RobotState.Shoot;
    }

    bool AgentAtDestination() {
        if (!agent.pathPending) {
            if (agent.remainingDistance <= agent.stoppingDistance) {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                    return true;
                }
            }
        }
        return false;
    }


    bool PlayerNearby() {
        if (target == null) return false;
        if (Vector3.Distance(target.position, transform.position) > lineOfSightRange) return false;
        return true;
    }

    void FindAndMoveToPlayer() {
        NavMesh.SamplePosition(target.position, out NavMeshHit hit, 10f, NavMesh.AllAreas);
        agent.destination = hit.position;
        agent.isStopped = false;
    }

    bool CheckPlayerLineOfSight() {
        if (Physics.Raycast(agentEyes.position, (target.position + new Vector3(0, playerScale.Value / 2, 0)) - agentEyes.position, out RaycastHit hit, lineOfSightRange, lineOfSightLayers)) {
            if (hit.transform.CompareTag("Player")) {
                return true;
            }
        }
        return false;
    }

    void PollForPlayer() {
        if (target != null) return;
        target = GameObject.FindWithTag("Player").transform;

    }
    public void AddForce(Vector3 point, Vector3 normalizedForceDirection, float sourceScale, float forceStrength) {
        if (RobotState.Dead == state) {   
            ragdollCenterMass.GetComponent<Rigidbody>().AddForceAtPosition(normalizedForceDirection * forceStrength * ragdollForceMultiplier, point, ForceMode.Impulse);
            return;
        }
    }
    
    public void Vacuum(Vector3 point, Vector3 normalizedForceDirection, float sourceScale, float forceStrength) {
        if (!vacuumable) return;

        if (transform.TryGetComponent(out Rigidbody rb)) {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            if (scalable != null) forceStrength /= Mathf.Pow(scalable.GetCurrentScale(), 6);
            //if (scalable != null) forceStrength *= 1 / scalable.GetCurrentScale();
            rb.AddForceAtPosition(normalizedForceDirection * forceStrength, point, ForceMode.Impulse);
            Debug.Log("VACUUMED with force " + forceStrength);
        }
    }
    
}

