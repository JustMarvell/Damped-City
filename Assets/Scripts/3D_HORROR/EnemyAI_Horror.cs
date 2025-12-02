using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using FMODUnity;
using FMOD.Studio;

public class EnemyAI_Horror : MonoBehaviour
{
    private enum EnemyState
    {
        Idle,
        Roam,
        Attack,
        Chase,
        Curious
    }

    [Header("Moveslkdfjlaksdfj")]
    public float normalSpeed = 4f;
    public float sprintSpeed = 6f;
    public float maxWalkRadius = 15f;
    public float maxStuckTime = 2f;
    public float minIdleTime = 3f;
    public float maxIdleTime = 5f;

    [Header("Detectionsdafjskldfj")]
    public float lineOfSightDistance = 10f;
    private float deffLineOfSightDistance;
    public float lineOfSightYOffset = 1f;
    public float fieldOfViewAngle = 60f;
    public LayerMask obstacleMask;
    public LayerMask playerMask;

    [Header("Rewspawn")]
    public Transform[] respawnPosition;

    [Header("Captureasdfasdf")]
    public GameObject captureCamera;
    public float captureTime = 2.7f;

    private NavMeshAgent navAgent;
    private Transform playerTransform;
    [SerializeField] private EnemyState currentState = EnemyState.Roam;
    private float stuckTimer;
    private float idleTimer;

    public Animator animator;

    public static bool IS_SEEING_PLAYER;
    public GameObject runAlert;
    [HideInInspector] public bool useLineOfSight = true;
    private bool hasAttacked = false;

    EnemyManager enemyManager;

    public delegate void OnChase();
    public OnChase onChaseCallback;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyManager = EnemyManager.instance;

        onChaseCallback += enemyManager.OnEnemyChase;

        navAgent.speed = normalSpeed;

        SetState(EnemyState.Roam);
        SetRandomDestination();
        useLineOfSight = true;
        deffLineOfSightDistance = lineOfSightDistance;
    }

    void Update()
    {
        if (!navAgent.isOnNavMesh)
            return;

        if (DetectPlayer() && !hasAttacked)
            SetState(EnemyState.Chase);

        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdle();
                break;
            case EnemyState.Roam:
                HandleRoaming();
                break;
            case EnemyState.Attack:
                HandleAttackOnce();
                break;
            case EnemyState.Chase:
                HandleChasing();
                break;
        }
    }

    void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * maxWalkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, maxWalkRadius, NavMesh.AllAreas))
            navAgent.SetDestination(hit.position);
    }

    bool DetectPlayer()
    {
        if (playerTransform == null) return false;

        Vector3 directionToPlayer = playerTransform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > lineOfSightDistance) return false;

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer.normalized);
        if (angleToPlayer > fieldOfViewAngle / 2f) return false;

        if (Physics.Raycast(transform.position + Vector3.up * lineOfSightYOffset, directionToPlayer.normalized, out RaycastHit hit, lineOfSightDistance, obstacleMask))
            if (!hit.collider.CompareTag("Player")) return false;

        return true;
    }

    void HandleIdle()
    {
        navAgent.isStopped = true;
        lineOfSightDistance = deffLineOfSightDistance;
        if (enemyManager.isChasingPlayer)
            enemyManager.isChasingPlayer = false;

        if (idleTimer <= 0f)
            idleTimer = Random.Range(minIdleTime, maxIdleTime);

        idleTimer -= Time.deltaTime;
        if (idleTimer <= 0f)
        {
            SetState(EnemyState.Roam);
            SetRandomDestination();
        }
    }

    void HandleRoaming()
    {
        navAgent.speed = normalSpeed;
        navAgent.isStopped = false;

        if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            SetState(EnemyState.Idle);
            idleTimer = 0f;
            return;
        }

        if (navAgent.velocity.sqrMagnitude < .01f)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer >= maxStuckTime)
            {
                SetRandomDestination();
                stuckTimer = 0f;
            }
        }
        else
            stuckTimer = 0f;
    }

    void HandleAttackOnce()
    {
        if (!hasAttacked)
        {
            hasAttacked = true;

            enemyManager.OnEnemyAttack();
        }
    }

    void HandleChasing()
    {
        navAgent.speed = sprintSpeed;
        navAgent.isStopped = false;
        navAgent.SetDestination(playerTransform.position);
        lineOfSightDistance = deffLineOfSightDistance + 5;

        enemyManager.isChasingPlayer = true;

        onChaseCallback?.Invoke();


        if (!DetectPlayer())
        {
            enemyManager.isChasingPlayer = false;
            
            onChaseCallback?.Invoke();
            SetState(EnemyState.Idle);
            idleTimer = 0f;
            return;
        }

        if (currentState == EnemyState.Attack)
        {
            return;
        }

        if (Physics.CheckSphere(transform.position, navAgent.stoppingDistance + 1, playerMask))
        {
            SetState(EnemyState.Attack);
        }
    }

    private void SetState(EnemyState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        string animName = GetAnimationName(newState);
        if (animator != null && !string.IsNullOrEmpty(animName))
        {
            animator.CrossFade(animName, 0.2f); 
        }

        if (newState == EnemyState.Chase)
        {
            IS_SEEING_PLAYER = true;
            runAlert.SetActive(true);
        }
        else
        {
            IS_SEEING_PLAYER = false;
            runAlert.SetActive(false);
        }

        if (!hasAttacked && newState == EnemyState.Attack && Vector3.Distance(transform.position, playerTransform.position) < navAgent.stoppingDistance + 1)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    private string GetAnimationName(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Idle:
                return "Idle";
            case EnemyState.Roam:
                return "Roam";
            case EnemyState.Chase:
                return "Chase";
            case EnemyState.Attack:
                return "Attack";
            default:
                return string.Empty;
        }
    }

    private IEnumerator AttackCoroutine()
    {
        if (!captureCamera.activeSelf)
            GameObject.FindGameObjectWithTag("charCam")?.SetActive(false);
        
        playerTransform.GetComponent<StateMachine_3D>().currentState.StopPlayerSounds();
        enemyManager.StopChasingSound();

        playerTransform.gameObject.SetActive(false);
        captureCamera.SetActive(true);
        yield return new WaitForSeconds(captureTime);
        captureCamera.SetActive(false);

        GameMaster.instance.GameOver();
        SetState(EnemyState.Idle);
    }

    void OnDrawGizmosSelected()
    {
        // maxWalkRadius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxWalkRadius);

        // field of fiew
        Gizmos.color = Color.red;
        
        // Left boundary
        Quaternion leftRotation = Quaternion.AngleAxis(-fieldOfViewAngle / 2, Vector3.up);
        Vector3 leftDirection = leftRotation * transform.forward * lineOfSightDistance;
        Gizmos.DrawRay(transform.position + Vector3.up * lineOfSightYOffset, leftDirection);

        // Right boundary
        Quaternion rightRotation = Quaternion.AngleAxis(fieldOfViewAngle / 2, Vector3.up);
        Vector3 rightDirection = rightRotation * transform.forward * lineOfSightDistance;
        Gizmos.DrawRay(transform.position + Vector3.up * lineOfSightYOffset, rightDirection);
    }
}
