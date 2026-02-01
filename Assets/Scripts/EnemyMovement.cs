using UnityEngine;

public enum AIType
{
    Melee,
    Ranged
}

public class EnemyMovement : MonoBehaviour
{
    public AIType aiType = AIType.Melee;

    public Transform target;
    public Rigidbody rb;

    public float moveSpeed = 3f;

    public float fallbackStopDistance = 1.6f;
    public float fallbackStopTolerance = 0.2f;

    float fixedPosZ;

    EnemyFireSpit enemyFireSpit;
    EnemyAttack enemyAttack;
    Animator animator;

    RigidbodyConstraints baseConstraints;

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        if (rb != null) baseConstraints = rb.constraints;

        fixedPosZ = transform.position.z;

        enemyFireSpit = GetComponent<EnemyFireSpit>();
        enemyAttack = GetComponent<EnemyAttack>();
        animator = GetComponent<Animator>();

        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) target = playerObj.transform;
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            // Player is dead, stop all movement and disable this script.
            if (rb != null) rb.linearVelocity = Vector3.zero;
            if (animator != null) animator.SetBool("isWalking", false);
            this.enabled = false;
            return;
        }
        if (rb == null) return;

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

        float maxStopDistance = stopDistanceFromPlayer + stopTolerance;

        float verticalVelocity = rb.linearVelocity.y;

        bool shouldHoldPosition = absHorizontalDistanceToTarget <= maxStopDistance;

        if (animator != null && animator.GetBool("isAttacking"))
        {
            shouldHoldPosition = true;
        }

        if (shouldHoldPosition)
        {
            rb.constraints = baseConstraints | RigidbodyConstraints.FreezePositionX;
            rb.linearVelocity = new Vector3(0f, verticalVelocity, 0f);
            if (animator != null) animator.SetBool("isWalking", false);
        }
        else
        {
            rb.constraints = baseConstraints & ~RigidbodyConstraints.FreezePositionX;

            float moveDirection = Mathf.Sign(horizontalDistanceToTarget);
            rb.linearVelocity = new Vector3(moveDirection * moveSpeed, verticalVelocity, 0f);
            if (animator != null) animator.SetBool("isWalking", true);
        }

        FaceTarget(horizontalDistanceToTarget);
    }

    void GetStopBand(out float stopDistanceFromPlayer, out float stopTolerance)
    {
        if (aiType == AIType.Melee && enemyAttack != null)
        {
            stopDistanceFromPlayer = enemyAttack.holdDistanceFromPlayer;
            stopTolerance = enemyAttack.holdTolerance;
            return;
        }

        if (aiType == AIType.Ranged && enemyFireSpit != null)
        {
            stopDistanceFromPlayer = enemyFireSpit.stopDistanceFromPlayer;
            stopTolerance = enemyFireSpit.stopTolerance;
            return;
        }

        stopDistanceFromPlayer = fallbackStopDistance;
        stopTolerance = fallbackStopTolerance;
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
