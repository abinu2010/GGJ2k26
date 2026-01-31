using UnityEngine;

public class EnemyFireSpit : MonoBehaviour
{
    public Transform target;
    public Transform mouthPoint;

    public FireProjectile projectilePrefab;

    public float engageRange = 7f;
    public float cooldown = 2f;

    public float idealDistanceFromPlayer = 5f;
    public float minDistanceFromPlayer = 3.5f;

    public float projectileSpeed = 10f;
    public float projectileDamage = 12f;
    public float projectileLifeTime = 2.5f;

    public LayerMask projectileHitLayers;

    float nextTime;

    void Update()
    {
        if (target == null) return;
        if (projectilePrefab == null) return;
        if (Time.time < nextTime) return;

        float horizontalDistanceToTarget = target.position.x - transform.position.x;
        float absHorizontalDistanceToTarget = Mathf.Abs(horizontalDistanceToTarget);

        if (absHorizontalDistanceToTarget > engageRange) return;
        if (absHorizontalDistanceToTarget < minDistanceFromPlayer) return;

        Spit(horizontalDistanceToTarget);
        nextTime = Time.time + cooldown;
    }

    void Spit(float horizontalDistanceToTarget)
    {
        Vector3 spawnPos = mouthPoint != null ? mouthPoint.position : transform.position;

        FireProjectile p = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        p.speed = projectileSpeed;
        p.damage = projectileDamage;
        p.lifeTime = projectileLifeTime;
        p.hitLayers = projectileHitLayers;
        p.SetDirection(horizontalDistanceToTarget);
    }

    void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position;

        Gizmos.color = new Color(1f, 0.9f, 0.2f, 1f);
        DrawHorizontalRange(origin, engageRange);

        Gizmos.color = new Color(0.2f, 0.9f, 1f, 1f);
        DrawHorizontalRange(origin, idealDistanceFromPlayer);

        Gizmos.color = new Color(1f, 0.2f, 0.2f, 1f);
        DrawHorizontalRange(origin, minDistanceFromPlayer);
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
