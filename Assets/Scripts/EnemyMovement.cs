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
        if (target == null || rb == null) return;

        Vector3 currentPos = rb.position;
        currentPos.z = fixedPosZ;
        rb.position = currentPos;

        float horizontalDistanceToTarget = target.position.x - currentPos.x;
        float absHorizontalDistanceToTarget = Mathf.Abs(horizontalDistanceToTarget);

        float minHoldDistance;
        float maxHoldDistance;
        GetHoldBand(out minHoldDistance, out maxHoldDistance);

        float desiredMoveDirection = 0f;

        if (absHorizontalDistanceToTarget > maxHoldDistance)
        {
            desiredMoveDirection = Mathf.Sign(horizontalDistanceToTarget);
        }
        else if (absHorizontalDistanceToTarget < minHoldDistance)
        {
            desiredMoveDirection = -Mathf.Sign(horizontalDistanceToTarget);
        }

        float verticalVelocity = rb.linearVelocity.y;

        if (desiredMoveDirection == 0f)
        {
            rb.linearVelocity = new Vector3(0f, verticalVelocity, 0f);
            return;
        }

        bool isBackingOff = absHorizontalDistanceToTarget < minHoldDistance;
        float speedToUse = isBackingOff ? backOffSpeed : moveSpeed;

        rb.linearVelocity = new Vector3(desiredMoveDirection * speedToUse, verticalVelocity, 0f);

        Vector3 s = transform.localScale;

        if (desiredMoveDirection > 0f && s.x < 0f) transform.localScale = new Vector3(-s.x, s.y, s.z);
        if (desiredMoveDirection < 0f && s.x > 0f) transform.localScale = new Vector3(-s.x, s.y, s.z);
    }

    void GetHoldBand(out float minHoldDistance, out float maxHoldDistance)
    {
        if (enemyFireSpit != null && enemyFireSpit.enabled)
        {
            float min = Mathf.Max(0f, enemyFireSpit.minDistanceFromPlayer);
            float max = Mathf.Max(min, enemyFireSpit.idealDistanceFromPlayer);
            minHoldDistance = min;
            maxHoldDistance = max;
            return;
        }

        if (enemyAttack != null && enemyAttack.enabled)
        {
            float d = Mathf.Max(0f, enemyAttack.holdDistanceFromPlayer);
            float t = Mathf.Max(0f, enemyAttack.holdTolerance);
            minHoldDistance = Mathf.Max(0f, d - t);
            maxHoldDistance = Mathf.Max(minHoldDistance, d + t);
            return;
        }

        minHoldDistance = Mathf.Max(0f, fallbackHoldDistance - fallbackHoldTolerance);
        maxHoldDistance = Mathf.Max(minHoldDistance, fallbackHoldDistance + fallbackHoldTolerance);
    }
}
