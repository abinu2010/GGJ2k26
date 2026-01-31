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

    float nextTime;

    void Update()
    {
        if (target == null) return;
        if (Time.time < nextTime) return;

        float horizontalDistanceToTarget = target.position.x - transform.position.x;
        float absHorizontalDistanceToTarget = Mathf.Abs(horizontalDistanceToTarget);

        if (absHorizontalDistanceToTarget > engageRange) return;

        Hit();
        nextTime = Time.time + cooldown;
    }

    void Hit()
    {
        Vector3 hitPos = hitPoint != null ? hitPoint.position : transform.position;

        Collider[] hits = Physics.OverlapSphere(hitPos, Mathf.Max(0f, hitRadius), playerLayer);

        for (int i = 0; i < hits.Length; i++)
        {
            Health Health = hits[i].GetComponentInParent<Health>();
            if (Health != null)
            {
                Health.addDamage(damage);
                return;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position;

        Gizmos.color = new Color(1f, 0.9f, 0.2f, 1f);
        DrawHorizontalRange(origin, engageRange);

        float innerHold = Mathf.Max(0f, holdDistanceFromPlayer - holdTolerance);
        float outerHold = Mathf.Max(innerHold, holdDistanceFromPlayer + holdTolerance);

        Gizmos.color = new Color(0.2f, 0.9f, 1f, 1f);
        DrawHorizontalRange(origin, outerHold);

        Gizmos.color = new Color(1f, 0.2f, 0.2f, 1f);
        DrawHorizontalRange(origin, innerHold);

        Vector3 hitPos = hitPoint != null ? hitPoint.position : transform.position;
        Gizmos.color = new Color(0.6f, 0.2f, 1f, 1f);
        Gizmos.DrawWireSphere(hitPos, Mathf.Max(0f, hitRadius));
    }

    void DrawHorizontalRange(Vector3 origin, float range)
    {
        float r = Mathf.Max(0f, range);

        Vector3 left = new Vector3(origin.x - r, origin.y, origin.z);
        Vector3 right = new Vector3(origin.x + r, origin.y, origin.z);

        Gizmos.DrawLine(left, right);
        Gizmos.DrawWireSphere(left, 0.08f);
        Gizmos.DrawWireSphere(right, 0.08f);
    }
}
