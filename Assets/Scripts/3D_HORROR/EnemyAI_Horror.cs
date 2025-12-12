using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using FMODUnity;
using FMOD.Studio;
using System.IO.Pipes;
using Unity.VisualScripting;

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

    public enum EnemyType
    {
        MutantSkull,
        MutantZombie,
        Scavanger
    }

    [Header("Enemy Type")]
    public EnemyType enemyType = EnemyType.MutantZombie;

    [Header("Moveslkdfjlaksdfj")]
    public float normalSpeed = 4f;
    public float sprintSpeed = 6f;
    public float maxWalkRadius = 15f;
    public float maxStuckTime = 3f;
    public float minIdleTime = 4f;
    public float maxIdleTime = 6f;

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

    public float chaseMemoryTime = 6f;

    private NavMeshAgent navAgent;
    private Transform playerTransform;
    [SerializeField] private EnemyState currentState = EnemyState.Roam;
    private float stuckTimer;
    private float idleTimer;

    public Animator animator;

    public static bool IS_SEEING_PLAYER;
    public GameObject runAlert;
    [HideInInspector] public bool useLineOfSight = true;

    public bool hasAttacked = false;
    private Vector3 lastKnownPosition = Vector3.zero;
    private float lastSightTime;

    EnemyManager enemyManager;

    public delegate void OnChase();
    public OnChase onChaseCallback;
    public delegate void OnCurious();
    public OnCurious onCuriousCallback;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyManager = EnemyManager.instance;

        onChaseCallback += enemyManager.OnEnemyChase;
        onCuriousCallback += enemyManager.OnEnemyCurious;

        navAgent.speed = normalSpeed;

        SetState(EnemyState.Roam);
        SetRandomDestination();
        useLineOfSight = true;
        deffLineOfSightDistance = lineOfSightDistance;
    }

    public void ApplyEnemyTypeVariations(DificultySettings settings)
    {
        switch (enemyType)
        {
            case EnemyType.MutantZombie:
                // Slow, wide blind sweeps, long memory, rare curious
                normalSpeed = settings.zombie_normalSpeed;
                sprintSpeed = settings.zombie_sprintSpeed;
                fieldOfViewAngle = settings.zombie_fieldOfViewAngle;
                lineOfSightDistance = settings.zombie_lineOfSightDistance;
                maxWalkRadius = settings.zombie_maxWalkRadius;
                minIdleTime = settings.zombie_minIdleTime;
                maxIdleTime = settings.zombie_maxIdleTime;
                useLineOfSight = settings.zombie_useLineOfSight;
                chaseMemoryTime = settings.zombie_chaseMemoryTime;
                navAgent.acceleration = settings.zombie_navAgentAcceleration;
                navAgent.angularSpeed = settings.zombie_navAgentAngularSpeed;
                break;

            case EnemyType.MutantSkull:
                // Fast, narrow/long sight, short memory, quick pivots
                normalSpeed = settings.skull_normalSpeed;
                sprintSpeed = settings.skull_sprintSpeed;
                fieldOfViewAngle = settings.skull_fieldOfViewAngle;
                lineOfSightDistance = settings.skull_lineOfSightDistance;
                maxWalkRadius = settings.skull_maxWalkRadius;
                minIdleTime = settings.skull_minIdleTime;
                maxIdleTime = settings.skull_maxIdleTime;
                useLineOfSight = settings.skull_useLineOfSight;
                chaseMemoryTime = settings.skull_chaseMemoryTime;
                navAgent.acceleration = settings.skull_navAgentAcceleration;
                navAgent.angularSpeed = settings.skull_navAgentAngularSpeed;
                break;

            case EnemyType.Scavanger:
                // Medium, sound-based (no LOS), erratic pauses, good memory
                normalSpeed = settings.scavanger_normalSpeed;
                sprintSpeed = settings.scavanger_sprintSpeed;
                fieldOfViewAngle = settings.scavanger_fieldOfViewAngle;
                lineOfSightDistance = settings.scavanger_lineOfSightDistance;
                maxWalkRadius = settings.scavanger_maxWalkRadius;
                minIdleTime = settings.scavanger_minIdleTime;
                maxIdleTime = settings.scavanger_maxIdleTime;
                useLineOfSight = settings.scavanger_useLineOfSight;
                chaseMemoryTime = settings.scavanger_chaseMemoryTime;
                navAgent.acceleration = settings.scavanger_navAgentAcceleration;
                navAgent.angularSpeed = settings.scavanger_navAgentAngularSpeed;
                break;
        }
    }

    void Update()
    {
        if (!navAgent.isOnNavMesh)
            return;

        if (DetectPlayer() && !hasAttacked)
        {
            lastKnownPosition = playerTransform.position;
            lastSightTime = Time.time;
            SetState(EnemyState.Chase);
        }

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
            case EnemyState.Curious:
                HandleCurious();
                break;
        }
    }

    private void HandleCurious()
    {
        navAgent.speed = normalSpeed;
        navAgent.isStopped = false;
        lineOfSightDistance = deffLineOfSightDistance + 3;
        onCuriousCallback?.Invoke();

        if (DetectPlayer())
        {
            onCuriousCallback?.Invoke();
            SetState(EnemyState.Chase);
            return;
        }

        if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            lastKnownPosition = Vector3.zero;
            onCuriousCallback.Invoke();
            SetState(EnemyState.Idle);
            idleTimer = 0f;
        }
    }

    NavMeshHit _hit;
    void SetRandomDestination()
    {
        // Vector3 randomDirection = Random.insideUnitSphere * maxWalkRadius;
        // randomDirection += transform.position;
        // NavMeshHit hit;
        // if (NavMesh.SamplePosition(randomDirection, out hit, maxWalkRadius, NavMesh.AllAreas))
        //     navAgent.SetDestination(hit.position);

        bool foundValidPosition = false;
        int attempts = 0;
        const int maxAttempts = 10;
        const float minRoamDistance = 5f;

        while (attempts < maxAttempts && !foundValidPosition)
        {
            Vector3 randomDirection = Random.insideUnitSphere * maxWalkRadius + transform.position;
            
            if (NavMesh.SamplePosition(randomDirection, out _hit, maxWalkRadius, NavMesh.AllAreas))
            {
                if (Vector3.Distance(transform.position, _hit.position) >= minRoamDistance)
                {
                    foundValidPosition = true;
                }
            }
            
            attempts++;
        }

        if (foundValidPosition)
        {
            navAgent.SetDestination(_hit.position);
        }
        else
        {
            Vector3 fallbackDir = Random.insideUnitSphere * (maxWalkRadius * 0.5f) + transform.position;
            NavMeshHit fallbackHit;
            if (NavMesh.SamplePosition(fallbackDir, out fallbackHit, maxWalkRadius * 0.5f, NavMesh.AllAreas))
            {
                navAgent.SetDestination(fallbackHit.position);
            }
            else
            {
                navAgent.ResetPath();
            }
        }
    }

    bool DetectPlayer()
    {
        if (playerTransform == null) return false;

        Vector3 playerPos = playerTransform.position + Vector3.up * lineOfSightYOffset;
        Vector3 directionToPlayer = (playerPos - (transform.position + Vector3.up * lineOfSightYOffset)).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, playerPos);

        if (distanceToPlayer > lineOfSightDistance)
            return false;

        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        if (angle > fieldOfViewAngle * 0.5f)
            return false;

        if (useLineOfSight)
        {
            return !Physics.Raycast(transform.position + Vector3.up * lineOfSightYOffset, directionToPlayer, distanceToPlayer, obstacleMask);
        }
        else
        {
            return true; 
        }
    }

    void HandleIdle()
    {
        navAgent.isStopped = true;
        lineOfSightDistance = deffLineOfSightDistance;

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
        
    }

    void HandleChasing()
    {
        navAgent.speed = sprintSpeed;
        navAgent.isStopped = false;
        lineOfSightDistance = deffLineOfSightDistance + 5;

        onChaseCallback?.Invoke();

        if (DetectPlayer())
        {
            onChaseCallback?.Invoke();

            navAgent.SetDestination(playerTransform.position);
            lastKnownPosition = playerTransform.position;
            lastSightTime = Time.time;
        }
        else if(Time.time - lastSightTime > chaseMemoryTime)
        {
            onChaseCallback?.Invoke();

            switch (enemyType)
            {
                case EnemyType.MutantZombie:
                    SetState(EnemyState.Roam);  // Zombies forget slowly, back to wandering
                    break;
                case EnemyType.MutantSkull:
                case EnemyType.Scavanger:
                    SetState(EnemyState.Curious);  // Hunters/Scavengers investigate last spot
                    break;
            }
            return;
        }
        else
        {
            onChaseCallback?.Invoke();

            navAgent.SetDestination(lastKnownPosition);
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


    [Space]
    [Space]
    [Header("Stuff")]

    public bool wasChasing;
    public bool willBeChasing;

    public bool wasCurious;
    public bool willCurious;

    private void SetState(EnemyState newState)
    {
        if (currentState == newState) return;

        wasChasing = currentState == EnemyState.Chase;
        willBeChasing = newState == EnemyState.Chase;

        wasCurious = currentState == EnemyState.Curious;
        willCurious = newState == EnemyState.Curious;

        if (wasChasing && !willBeChasing)
            enemyManager.StopChasing();
        if (!wasChasing && willBeChasing)
            enemyManager.StartChasing();

        if (wasCurious && !willCurious)
            enemyManager.StopCurious();
        if (!wasCurious && willCurious)
            enemyManager.StartCurious();

        string animName = GetAnimationName(newState);
        if (animator != null && !string.IsNullOrEmpty(animName))
            animator.CrossFade(animName, 0.2f); 

        if (newState == EnemyState.Chase)
        {
            IS_SEEING_PLAYER = true;
            if (runAlert != null) runAlert.SetActive(true);
        }
        else
        {
            IS_SEEING_PLAYER = false;
            if (runAlert != null) runAlert.SetActive(false);
        }

        currentState = newState;

        if (!hasAttacked && newState == EnemyState.Attack && Physics.CheckSphere(transform.position, navAgent.stoppingDistance + 1, playerMask))
            StartCoroutine(AttackCoroutine());
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
            case EnemyState.Curious:
                return "Roam";
            default:
                return string.Empty;
        }
    }

    private IEnumerator AttackCoroutine()
    {
        if (!hasAttacked)
        {
            hasAttacked = true;

            enemyManager.OnEnemyAttack();
        }
            
        onChaseCallback?.Invoke();
        onCuriousCallback?.Invoke();

        if (!captureCamera.activeSelf)
            GameObject.FindGameObjectWithTag("charCam")?.SetActive(false);
        
        playerTransform.GetComponent<StateMachine_3D>().currentState.StopPlayerSounds();
        enemyManager.StopChasingSound();
        enemyManager.StopCuriousSound();

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
