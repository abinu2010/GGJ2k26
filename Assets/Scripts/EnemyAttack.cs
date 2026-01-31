using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform target;
    public Transform hitPoint;
    public LayerMask playerLayer;

    public float engageRange = 1.0f;
    public float hitRadius = 0.6f;

    public float holdDistanceFromPlayer = 1.4f;
    public float holdTolerance = 0.25f;

    public float damage = 10f;
    public float cooldown = 1.2f;

    float nextAttackTime;

    void Update()
    {
        if (target == null) return;
        if (Time.time < nextAttackTime) return;

        float posX = transform.position.x;
        float targetPosX = target.position.x;

        float horizontalDistanceToTarget = targetPosX - posX;
        float absHorizontalDistanceToTarget = Mathf.Abs(horizontalDistanceToTarget);

        if (absHorizontalDistanceToTarget > engageRange) return;

        PerformHit();
        nextAttackTime = Time.time + cooldown;
    }

    void PerformHit()
    {
        float hitPosX = hitPoint != null ? hitPoint.position.x : transform.position.x;
        float hitPosY = hitPoint != null ? hitPoint.position.y : transform.position.y;
        float hitPosZ = hitPoint != null ? hitPoint.position.z : transform.position.z;

        Vector3 hitPosition = new Vector3(hitPosX, hitPosY, hitPosZ);

        Collider[] hitColliders = Physics.OverlapSphere(hitPosition, Mathf.Max(0f, hitRadius), playerLayer);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i] == null) continue;

            Health health = hitColliders[i].GetComponentInParent<Health>();
            if (health != null)
            {
                health.addDamage(damage);
                return;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        float posX = transform.position.x;
        float posY = transform.position.y;
        float posZ = transform.position.z;

        Vector3 origin = new Vector3(posX, posY, posZ);

        Gizmos.color = new Color(1f, 0.9f, 0.2f, 1f);
        DrawHorizontalRange(origin, engageRange);

        float innerHoldDistance = Mathf.Max(0f, holdDistanceFromPlayer - holdTolerance);
        float outerHoldDistance = Mathf.Max(innerHoldDistance, holdDistanceFromPlayer + holdTolerance);

        Gizmos.color = new Color(0.2f, 0.9f, 1f, 1f);
        DrawHorizontalRange(origin, outerHoldDistance);

        Gizmos.color = new Color(1f, 0.2f, 0.2f, 1f);
        DrawHorizontalRange(origin, innerHoldDistance);

        float hitPosX = hitPoint != null ? hitPoint.position.x : transform.position.x;
        float hitPosY = hitPoint != null ? hitPoint.position.y : transform.position.y;
        float hitPosZ = hitPoint != null ? hitPoint.position.z : transform.position.z;

        Gizmos.color = new Color(0.6f, 0.2f, 1f, 1f);
        Gizmos.DrawWireSphere(new Vector3(hitPosX, hitPosY, hitPosZ), Mathf.Max(0f, hitRadius));
    }

    void DrawHorizontalRange(Vector3 origin, float range)
    {
        float safeRange = Mathf.Max(0f, range);

        Vector3 left = new Vector3(origin.x - safeRange, origin.y, origin.z);
        Vector3 right = new Vector3(origin.x + safeRange, origin.y, origin.z);

        Gizmos.DrawLine(left, right);
        Gizmos.DrawWireSphere(left, 0.08f);
        Gizmos.DrawWireSphere(right, 0.08f);
    }
}
