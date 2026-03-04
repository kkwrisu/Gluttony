using UnityEngine;
using UnityEngine.AI;

public class Enemy_Movement : MonoBehaviour
{
    [Header("Combate")]
    public float attackRange = 1.5f;
    public float playerDetectRange = 5f;
    public float attackCooldown = 2f;

    [Header("Campo de Visão")]
    [Range(0f, 180f)] public float visionAngle = 45f;
    public float chaseVisionRadius = 6f;
    public LayerMask visionBlockMask;

    [Header("Patrulha")]
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    private bool isReturning = false;

    [Header("Referências")]
    public Transform detectionPoint;
    public LayerMask playerLayer;

    private Transform player;
    private Animator anim;
    private NavMeshAgent agent;

    private float attackCooldownTimer;
    private bool canAttack = true;

    private EnemyState enemyState;

    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        ChangeState(EnemyState.Patrolling);
    }

    private void Update()
    {
        HandleCooldown();
        CheckForPlayer();

        switch (enemyState)
        {
            case EnemyState.Patrolling:
                Patrol();
                break;

            case EnemyState.Chasing:
                HandleChase();
                break;
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        Vector2 velocity = agent.velocity;

        if (velocity.magnitude > 0.1f)
        {
            anim.SetBool("isWalking", true);

            Vector2 dir = velocity.normalized;

            anim.SetFloat("InputX", dir.x);
            anim.SetFloat("InputY", dir.y);
            anim.SetFloat("LastInputX", dir.x);
            anim.SetFloat("LastInputY", dir.y);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    private void HandleChase()
    {
        if (player == null)
        {
            ChangeState(EnemyState.Patrolling);
            return;
        }

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist > chaseVisionRadius)
        {
            player = null;
            ChangeState(EnemyState.Patrolling);
            return;
        }

        agent.SetDestination(player.position);

        if (dist <= attackRange && canAttack)
        {
            Attack();
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0)
            return;

        agent.SetDestination(patrolPoints[currentPatrolIndex].position);

        if (!agent.pathPending && agent.remainingDistance < 0.2f)
        {
            if (!isReturning)
            {
                currentPatrolIndex++;
                if (currentPatrolIndex >= patrolPoints.Length - 1)
                {
                    currentPatrolIndex = patrolPoints.Length - 1;
                    isReturning = true;
                }
            }
            else
            {
                currentPatrolIndex--;
                if (currentPatrolIndex <= 0)
                {
                    currentPatrolIndex = 0;
                    isReturning = false;
                }
            }
        }
    }

    private void CheckForPlayer()
    {
        if (detectionPoint == null)
            return;

        Collider2D hit = Physics2D.OverlapCircle(
            detectionPoint.position,
            playerDetectRange,
            playerLayer);

        if (hit != null)
        {
            player = hit.transform;
            ChangeState(EnemyState.Chasing);
        }
    }

    private void HandleCooldown()
    {
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
            if (attackCooldownTimer <= 0)
                canAttack = true;
        }
    }

    private void Attack()
    {
        canAttack = false;
        attackCooldownTimer = attackCooldown;

        GetComponent<EnemyCombat>()?.Attack();
    }

    private void ChangeState(EnemyState newState)
    {
        enemyState = newState;
    }
}

public enum EnemyState
{
    Patrolling,
    Chasing
}