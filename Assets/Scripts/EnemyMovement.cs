using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public Rigidbody rb;

    public float moveSpeed = 3f;

    public float fallbackStopDistance = 1.6f;
    public float fallbackStopTolerance = 0.2f;

    float fixedPosZ;

    EnemyFireSpit enemyFireSpit;
    EnemyAttack enemyAttack;
    Animator animator;

    void Awake()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (target == null)
            {
                Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
            }
        }
        if (rb == null) rb = GetComponent<Rigidbody>();
        fixedPosZ = transform.position.z;

        enemyFireSpit = GetComponent<EnemyFireSpit>();
        enemyAttack = GetComponent<EnemyAttack>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (target == null) return;
        if (rb == null) return;

        // Halt all horizontal movement if the enemy is in the middle of an attack animation.
        if (animator != null && animator.GetBool("isAttacking"))
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            if (animator != null) animator.SetBool("isWalking", false);
            return;
        }

        float posX = rb.position.x;
        float posY = rb.position.y;
        float posZ = fixedPosZ;

        rb.position = new Vector3(posX, posY, posZ);

        float targetPosX = target.position.x;

        float horizontalDistanceToTarget = targetPosX - posX;
        float absHorizontalDistanceToTarget = Mathf.Abs(horizontalDistanceToTarget);

        float stopDistanceFromPlayer;
        float stopTolerance;
        GetStopBand(out stopDistanceFromPlayer, out stopTolerance);

        float minStopDistance = Mathf.Max(0f, stopDistanceFromPlayer - stopTolerance);
        float maxStopDistance = Mathf.Max(minStopDistance, stopDistanceFromPlayer + stopTolerance);

        if(animator != null)
        {
            Debug.Log("Distance: " + absHorizontalDistanceToTarget + " | MaxStop: " + maxStopDistance + " | isAttacking: " + animator.GetBool("isAttacking"));
        }

        float verticalVelocity = rb.linearVelocity.y;

        if (absHorizontalDistanceToTarget <= maxStopDistance)
        {
            rb.linearVelocity = new Vector3(0f, verticalVelocity, 0f);
            FaceTarget(horizontalDistanceToTarget);
            if (animator != null) animator.SetBool("isWalking", false);
            return;
        }

        float moveDirection = Mathf.Sign(horizontalDistanceToTarget);
        rb.linearVelocity = new Vector3(moveDirection * moveSpeed, verticalVelocity, 0f);
        if (animator != null) animator.SetBool("isWalking", true);

        FaceTarget(horizontalDistanceToTarget);
    }

    void GetStopBand(out float stopDistanceFromPlayer, out float stopTolerance)
    {
        if (enemyFireSpit != null && enemyFireSpit.enabled)
        {
            stopDistanceFromPlayer = Mathf.Max(0f, enemyFireSpit.stopDistanceFromPlayer);
            stopTolerance = Mathf.Max(0f, enemyFireSpit.stopTolerance);
            return;
        }

        if (enemyAttack != null && enemyAttack.enabled)
        {
            stopDistanceFromPlayer = Mathf.Max(0f, enemyAttack.holdDistanceFromPlayer);
            stopTolerance = Mathf.Max(0f, enemyAttack.holdTolerance);
            return;
        }

        stopDistanceFromPlayer = Mathf.Max(0f, fallbackStopDistance);
        stopTolerance = Mathf.Max(0f, fallbackStopTolerance);
    }

    void FaceTarget(float horizontalDistanceToTarget)
    {
        float moveDirection = Mathf.Sign(horizontalDistanceToTarget);

        float scaleX = transform.localScale.x;
        float scaleY = transform.localScale.y;
        float scaleZ = transform.localScale.z;

        if (moveDirection > 0f && scaleX < 0f) transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
        if (moveDirection < 0f && scaleX > 0f) transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
    }
}
