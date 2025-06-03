using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    [System.Serializable]
    public enum EnemyState
    {
        Patrol,
        Attack,
        Return
    }

    [Header("Debug Settings")]
    public bool enableDebugLogs = true;
    public bool showDebugGizmos = true;
    public string enemyName = "Enemy"; // Untuk membedakan log antar enemy

    [Header("State Management")]
    public EnemyState currentState;
    
    [Header("Player Detection")]
    public Transform player;
    public float detectionRange = 8f;
    public float attackRange = 5f;
    
    [Header("Patrol Settings")]
    public bool useSimplePatrol = true;
    public float patrolSpeed = 2f;
    public float patrolDistance = 3f;
    public Transform[] patrolPoints;
    
    [Header("Ground Detection")]
    public Transform groundDetection;
    public LayerMask groundLayer = 1;
    public float groundCheckDistance = 0.5f;
    public bool useGroundCheck = true;
    
    [Header("Attack Settings")]
    public float attackCooldown = 2f;
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireballSpeed = 7f;
    
    [Header("Return Settings")]
    public float returnSpeed = 3f;
    
    // Private variables
    private Vector3 leftEdge;
    private Vector3 rightEdge;
    private Vector3 originalPosition;
    private int currentPatrolPoint = 0;
    private bool movingRight = true;
    private float attackTimer = 0f;
    private bool isGrounded = true;
    private float stateTimer = 0f;
    
    // Debug variables
    private float debugTimer = 0f;
    private Vector2 lastPosition;
    private bool isStuck = false;
    
    // Components
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D col;

    void Start()
    {
        // Set enemy name if not set
        if (string.IsNullOrEmpty(enemyName))
        {
            enemyName = gameObject.name;
        }
        
        InitializeComponents();
        ValidateSetup();
        InitializePatrol();
        FindPlayer();
        ChangeState(EnemyState.Patrol);
        
        lastPosition = transform.position;
        
        DebugLog("Enemy initialized successfully");
    }

    void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        
        originalPosition = transform.position;
        
        // Validate Rigidbody2D settings
        if (rb != null)
        {
            rb.freezeRotation = true; // Prevent rotation
            if (rb.gravityScale == 0)
            {
                DebugLog("WARNING: Rigidbody2D gravity scale is 0, enemy might not fall properly");
            }
        }
        
        // Setup ground detection if not assigned
        if (groundDetection == null && useGroundCheck)
        {
            GameObject groundCheck = new GameObject("GroundCheck_" + enemyName);
            groundCheck.transform.SetParent(transform);
            
            float yOffset = col != null ? -col.bounds.extents.y - 0.1f : -0.6f;
            groundCheck.transform.localPosition = new Vector3(0, yOffset, 0);
            groundDetection = groundCheck.transform;
            
            DebugLog("Auto-created ground detection point");
        }
    }

    void ValidateSetup()
    {
        List<string> issues = new List<string>();
        
        if (rb == null) issues.Add("Missing Rigidbody2D component");
        if (spriteRenderer == null) issues.Add("Missing SpriteRenderer component");
        if (col == null) issues.Add("Missing Collider2D component");
        if (useGroundCheck && groundDetection == null) issues.Add("Ground detection not set");
        if (patrolDistance <= 0) issues.Add("Patrol distance is 0 or negative");
        if (patrolSpeed <= 0) issues.Add("Patrol speed is 0 or negative");
        
        if (issues.Count > 0)
        {
            DebugLog("SETUP ISSUES FOUND:");
            foreach (string issue in issues)
            {
                DebugLog("- " + issue);
            }
        }
    }

    void InitializePatrol()
    {
        if (useSimplePatrol)
        {
            leftEdge = originalPosition - new Vector3(patrolDistance, 0, 0);
            rightEdge = originalPosition + new Vector3(patrolDistance, 0, 0);
            
            DebugLog($"Patrol setup - Left: {leftEdge.x:F2}, Right: {rightEdge.x:F2}, Distance: {patrolDistance}");
        }
        else
        {
            if (patrolPoints == null || patrolPoints.Length == 0)
            {
                DebugLog("WARNING: Waypoint patrol enabled but no patrol points assigned!");
                useSimplePatrol = true; // Fallback to simple patrol
                InitializePatrol(); // Reinitialize with simple patrol
            }
        }
    }

    void FindPlayer()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                DebugLog("Player found: " + player.name);
            }
            else
            {
                DebugLog("WARNING: Player with tag 'Player' not found!");
            }
        }
    }

    void Update()
    {
        stateTimer += Time.deltaTime;
        debugTimer += Time.deltaTime;
        
        CheckGround();
        CheckIfStuck();
        
        if (player != null)
        {
            HandlePlayerDetection();
        }
        else
        {
            FindPlayer();
            if (currentState != EnemyState.Patrol)
            {
                ChangeState(EnemyState.Patrol);
            }
        }
        
        ExecuteCurrentState();
        
        // Debug info every 2 seconds
        if (debugTimer >= 2f)
        {
            LogDebugInfo();
            debugTimer = 0f;
        }
        
        lastPosition = transform.position;
    }

    void CheckIfStuck()
    {
        float distanceMoved = Vector2.Distance(transform.position, lastPosition);
        
        // If enemy should be moving but hasn't moved much
        if (currentState == EnemyState.Patrol && distanceMoved < 0.01f && rb.linearVelocity.magnitude < 0.1f)
        {
            if (!isStuck)
            {
                isStuck = true;
                DebugLog("WARNING: Enemy appears to be stuck!");
                
                // Try to unstuck
                movingRight = !movingRight;
                DebugLog("Attempting to change direction to unstuck");
            }
        }
        else
        {
            isStuck = false;
        }
    }

    void CheckGround()
    {
        if (useGroundCheck && groundDetection != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                groundDetection.position, 
                Vector2.down, 
                groundCheckDistance, 
                groundLayer
            );
            
            bool wasGrounded = isGrounded;
            isGrounded = hit.collider != null;
            
            if (wasGrounded != isGrounded)
            {
                DebugLog($"Ground state changed: {(isGrounded ? "Grounded" : "Not Grounded")}");
            }
        }
        else
        {
            isGrounded = true;
        }
    }

    void HandlePlayerDetection()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (stateTimer < 0.2f) return;
        
        switch (currentState)
        {
            case EnemyState.Patrol:
                if (distanceToPlayer <= attackRange)
                {
                    ChangeState(EnemyState.Attack);
                }
                break;

            case EnemyState.Attack:
                if (distanceToPlayer > detectionRange)
                {
                    ChangeState(EnemyState.Return);
                }
                break;

            case EnemyState.Return:
                float distanceToHome = Vector2.Distance(transform.position, originalPosition);
                if (distanceToHome < 0.5f)
                {
                    ChangeState(EnemyState.Patrol);
                }
                else if (distanceToPlayer <= attackRange)
                {
                    ChangeState(EnemyState.Attack);
                }
                break;
        }
    }

    void ExecuteCurrentState()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                PatrolBehavior();
                break;
            case EnemyState.Attack:
                AttackBehavior();
                break;
            case EnemyState.Return:
                ReturnBehavior();
                break;
        }
    }

    void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;
        
        EnemyState previousState = currentState;
        ExitState(currentState);
        currentState = newState;
        stateTimer = 0f;
        EnterState(newState);
        
        DebugLog($"State Changed: {previousState} -> {newState}");
    }

    void ExitState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Attack:
                attackTimer = 0;
                break;
        }
    }

    void EnterState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Patrol:
                if (animator != null) animator.SetTrigger("Patrol");
                break;
            case EnemyState.Attack:
                if (animator != null) animator.SetTrigger("Attack");
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                break;
            case EnemyState.Return:
                if (animator != null) animator.SetTrigger("Return");
                break;
        }
    }

    void PatrolBehavior()
    {
        if (useSimplePatrol)
        {
            SimplePatrol();
        }
        else
        {
            WaypointPatrol();
        }
    }

    void SimplePatrol()
    {
        bool shouldTurn = false;
        string turnReason = "";
        
        if (movingRight)
        {
            if (transform.position.x >= rightEdge.x)
            {
                shouldTurn = true;
                turnReason = "reached right boundary";
            }
            else if (useGroundCheck && !CheckGroundAhead(Vector2.right))
            {
                shouldTurn = true;
                turnReason = "no ground ahead (right)";
            }
        }
        else
        {
            if (transform.position.x <= leftEdge.x)
            {
                shouldTurn = true;
                turnReason = "reached left boundary";
            }
            else if (useGroundCheck && !CheckGroundAhead(Vector2.left))
            {
                shouldTurn = true;
                turnReason = "no ground ahead (left)";
            }
        }

        if (shouldTurn)
        {
            movingRight = !movingRight;
            FlipSprite();
            DebugLog($"Turning around: {turnReason}");
        }

        // Move
        float moveDirection = movingRight ? 1f : -1f;
        Vector2 newVelocity = new Vector2(moveDirection * patrolSpeed, rb.linearVelocity.y);
        rb.linearVelocity = newVelocity;
        
        // Debug current movement
        if (debugTimer >= 1.9f) // Log just before the 2-second debug info
        {
            DebugLog($"Patrol - Pos: {transform.position.x:F2}, Moving: {(movingRight ? "Right" : "Left")}, Velocity: {rb.linearVelocity.x:F2}");
        }
    }

    bool CheckGroundAhead(Vector2 direction)
    {
        if (groundDetection == null) return true;
        
        Vector2 checkPosition = (Vector2)groundDetection.position + direction * 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(checkPosition, Vector2.down, groundCheckDistance, groundLayer);
        
        bool hasGround = hit.collider != null;
        if (!hasGround)
        {
            DebugLog($"No ground detected ahead in direction: {direction}");
        }
        
        return hasGround;
    }

    void WaypointPatrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolPoint];
        if (targetPoint == null)
        {
            DebugLog($"WARNING: Patrol point {currentPatrolPoint} is null!");
            return;
        }

        Vector2 direction = (targetPoint.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * patrolSpeed, rb.linearVelocity.y);

        if (Mathf.Abs(direction.x) > 0.1f)
        {
            movingRight = direction.x > 0;
            FlipSprite();
        }

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.3f)
        {
            currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
            DebugLog($"Reached patrol point, moving to point {currentPatrolPoint}");
        }
    }

    void AttackBehavior()
    {
        if (player == null) return;

        FlipTowardsPlayer();
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        if (attackTimer <= 0)
        {
            FireFireball();
            attackTimer = attackCooldown;
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    void ReturnBehavior()
    {
        Vector2 direction = (originalPosition - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * returnSpeed, rb.linearVelocity.y);

        if (Mathf.Abs(direction.x) > 0.1f)
        {
            movingRight = direction.x > 0;
            FlipSprite();
        }
    }

    void FireFireball()
    {
        if (fireballPrefab == null || firePoint == null || player == null) return;

        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (player.position - firePoint.position).normalized;
        
        Rigidbody2D fireballRb = fireball.GetComponent<Rigidbody2D>();
        if (fireballRb != null)
        {
            fireballRb.linearVelocity = direction * fireballSpeed;
        }
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        fireball.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        Destroy(fireball, 5f);
        DebugLog("Fireball fired!");
    }

    void FlipTowardsPlayer()
    {
        if (player == null) return;
        
        bool shouldFaceRight = player.position.x > transform.position.x;
        if (movingRight != shouldFaceRight)
        {
            movingRight = shouldFaceRight;
            FlipSprite();
        }
    }

    void FlipSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !movingRight;
        }
    }

    void LogDebugInfo()
    {
        if (!enableDebugLogs) return;
        
        string debugInfo = $"[{enemyName}] State: {currentState}, Pos: {transform.position.x:F2}, " +
                          $"Velocity: {rb.linearVelocity.x:F2}, Moving: {(movingRight ? "Right" : "Left")}, " +
                          $"Grounded: {isGrounded}, Stuck: {isStuck}";
        
        if (player != null)
        {
            float playerDist = Vector2.Distance(transform.position, player.position);
            debugInfo += $", PlayerDist: {playerDist:F2}";
        }
        
        DebugLog(debugInfo);
    }

    void DebugLog(string message)
    {
        if (enableDebugLogs)
        {
            Debug.Log($"[{enemyName}] {message}");
        }
    }

    // Enhanced Gizmos for debugging
    void OnDrawGizmosSelected()
    {
        if (!showDebugGizmos) return;
        
        // Attack range (red)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        // Detection range (yellow)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Patrol boundaries
        if (useSimplePatrol)
        {
            Gizmos.color = Color.green;
            Vector3 center = Application.isPlaying ? originalPosition : transform.position;
            Vector3 left = center - new Vector3(patrolDistance, 0, 0);
            Vector3 right = center + new Vector3(patrolDistance, 0, 0);
            
            Gizmos.DrawLine(left + Vector3.up * 0.5f, left - Vector3.up * 0.5f);
            Gizmos.DrawLine(right + Vector3.up * 0.5f, right - Vector3.up * 0.5f);
            Gizmos.DrawLine(left, right);
            
            // Current position marker
            Gizmos.color = movingRight ? Color.cyan : Color.magenta;
            Gizmos.DrawSphere(transform.position, 0.1f);
        }
        
        // Ground check rays
        if (useGroundCheck && groundDetection != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawRay(groundDetection.position, Vector2.down * groundCheckDistance);
            
            // Ground check ahead
            Vector2 checkDir = movingRight ? Vector2.right : Vector2.left;
            Vector2 checkPos = (Vector2)groundDetection.position + checkDir * 0.5f;
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(checkPos, Vector2.down * groundCheckDistance);
        }
        
        // Patrol points
        if (patrolPoints != null)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] != null)
                {
                    Gizmos.DrawSphere(patrolPoints[i].position, 0.2f);
                    if (i == currentPatrolPoint)
                    {
                        Gizmos.color = Color.white;
                        Gizmos.DrawWireSphere(patrolPoints[i].position, 0.3f);
                        Gizmos.color = Color.blue;
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (!showDebugGizmos) return;
        
        // Show enemy name and state
        if (Application.isPlaying)
        {
            Vector3 textPos = transform.position + Vector3.up * 2f;
            
#if UNITY_EDITOR
            string displayText = $"{enemyName}\nState: {currentState}";
            if (isStuck) displayText += "\n(STUCK!)";
            
            UnityEditor.Handles.Label(textPos, displayText);
#endif
        }
    }
}