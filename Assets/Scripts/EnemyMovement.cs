using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public Rigidbody rb;

    public float moveSpeed = 3f;
    public float backOffSpeed = 4f;

    public float fallbackHoldDistance = 1.2f;
    public float fallbackHoldTolerance = 0.2f;

    float fixedPosZ;

    EnemyAttack enemyAttack;
    EnemyFireSpit enemyFireSpit;

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        fixedPosZ = transform.position.z;

        enemyAttack = GetComponent<EnemyAttack>();
        enemyFireSpit = GetComponent<EnemyFireSpit>();
    }

    void FixedUpdate()
    {
        if (target == null) return;
        if (rb == null) return;

        float posX = rb.position.x;
        float posY = rb.position.y;
        float posZ = fixedPosZ;

        rb.position = new Vector3(posX, posY, posZ);

        float targetPosX = target.position.x;

        float horizontalDistanceToTarget = targetPosX - posX;
        float absHorizontalDistanceToTarget = Mathf.Abs(horizontalDistanceToTarget);

        float minHoldDistance;
        float maxHoldDistance;
        GetHoldBand(out minHoldDistance, out maxHoldDistance);

        float moveDirection = 0f;

        if (absHorizontalDistanceToTarget > maxHoldDistance)
        {
            moveDirection = Mathf.Sign(horizontalDistanceToTarget);
        }
        else if (absHorizontalDistanceToTarget < minHoldDistance)
        {
            moveDirection = -Mathf.Sign(horizontalDistanceToTarget);
        }

        float verticalVelocity = rb.linearVelocity.y;

        if (moveDirection == 0f)
        {
            rb.linearVelocity = new Vector3(0f, verticalVelocity, 0f);
            return;
        }

        bool backingOff = absHorizontalDistanceToTarget < minHoldDistance;
        float speedToUse = backingOff ? backOffSpeed : moveSpeed;

        rb.linearVelocity = new Vector3(moveDirection * speedToUse, verticalVelocity, 0f);

        float scaleX = transform.localScale.x;
        float scaleY = transform.localScale.y;
        float scaleZ = transform.localScale.z;

        if (moveDirection > 0f && scaleX < 0f) transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
        if (moveDirection < 0f && scaleX > 0f) transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
    }

    void GetHoldBand(out float minHoldDistance, out float maxHoldDistance)
    {
        if (enemyFireSpit != null && enemyFireSpit.enabled)
        {
            float minDistance = Mathf.Max(0f, enemyFireSpit.minDistanceFromPlayer);
            float maxDistance = Mathf.Max(minDistance, enemyFireSpit.idealDistanceFromPlayer);

            minHoldDistance = minDistance;
            maxHoldDistance = maxDistance;
            return;
        }

        if (enemyAttack != null && enemyAttack.enabled)
        {
            float holdDistanceFromPlayer = Mathf.Max(0f, enemyAttack.holdDistanceFromPlayer);
            float holdTolerance = Mathf.Max(0f, enemyAttack.holdTolerance);

            minHoldDistance = Mathf.Max(0f, holdDistanceFromPlayer - holdTolerance);
            maxHoldDistance = Mathf.Max(minHoldDistance, holdDistanceFromPlayer + holdTolerance);
            return;
        }

        minHoldDistance = Mathf.Max(0f, fallbackHoldDistance - fallbackHoldTolerance);
        maxHoldDistance = Mathf.Max(minHoldDistance, fallbackHoldDistance + fallbackHoldTolerance);
    }
}
