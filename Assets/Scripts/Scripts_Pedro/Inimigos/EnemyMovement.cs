using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    [Header("Movimentação e Combate")]
    public float speed = 2f;
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
    public float patrolWaitTime = 2f;
    private float patrolWaitTimer = 0f;

    [Header("Referências")]
    public Transform attackPoint;
    public Transform detectionPoint;
    public LayerMask playerLayer;

    private Rigidbody2D rb;
    private Transform player;

    private float attackCooldownTimer;
    private bool canAttack = true;

    private EnemyState enemyState;
    private Vector2 facingDirection = Vector2.right;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

        if (dist > attackRange)
        {
            MoveTowards(player.position);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;

            if (canAttack)
                Attack();
        }
    }

    private void MoveTowards(Vector2 target)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * speed;

        if (dir != Vector2.zero)
            facingDirection = dir;
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0)
            return;

        Transform target = patrolPoints[currentPatrolIndex];
        MoveTowards(target.position);

        float dist = Vector2.Distance(transform.position, target.position);

        if (dist < 0.2f)
        {
            rb.linearVelocity = Vector2.zero;

            patrolWaitTimer += Time.deltaTime;

            if (patrolWaitTimer >= patrolWaitTime)
            {
                patrolWaitTimer = 0f;
                currentPatrolIndex++;

                if (currentPatrolIndex >= patrolPoints.Length)
                    currentPatrolIndex = 0;
            }
        }
    }

    private void CheckForPlayer()
    {
        if (detectionPoint == null)
            return;

        float detectRange =
            enemyState == EnemyState.Chasing
            ? chaseVisionRadius
            : playerDetectRange;

        Collider2D hit = Physics2D.OverlapCircle(
            detectionPoint.position,
            detectRange,
            playerLayer);

        if (hit == null)
        {
            if (enemyState == EnemyState.Chasing)
            {
                player = null;
                ChangeState(EnemyState.Patrolling);
            }
            return;
        }

        Vector2 dirToPlayer =
            (hit.transform.position - detectionPoint.position).normalized;

        float distance =
            Vector2.Distance(detectionPoint.position, hit.transform.position);

        if (enemyState == EnemyState.Patrolling)
        {
            float angle = Vector2.Angle(facingDirection, dirToPlayer);

            if (angle > visionAngle)
                return;

            RaycastHit2D block = Physics2D.Raycast(
                detectionPoint.position,
                dirToPlayer,
                distance,
                visionBlockMask);

            if (block.collider != null)
                return;
        }

        player = hit.transform;
        ChangeState(EnemyState.Chasing);
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

    private void OnDrawGizmos()
    {
        if (detectionPoint == null)
            return;

        Vector3 forward =
            facingDirection == Vector2.zero ? Vector2.right : facingDirection;

        if (!Application.isPlaying || enemyState == EnemyState.Patrolling)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(detectionPoint.position, playerDetectRange);

            Vector3 leftDir =
                Quaternion.Euler(0, 0, visionAngle) * forward;

            Vector3 rightDir =
                Quaternion.Euler(0, 0, -visionAngle) * forward;

            Gizmos.color = Color.red;

            Gizmos.DrawLine(
                detectionPoint.position,
                detectionPoint.position + leftDir * playerDetectRange);

            Gizmos.DrawLine(
                detectionPoint.position,
                detectionPoint.position + rightDir * playerDetectRange);
        }
        else if (enemyState == EnemyState.Chasing)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(detectionPoint.position, chaseVisionRadius);
        }

        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}

public enum EnemyState
{
    Patrolling,
    Chasing
}