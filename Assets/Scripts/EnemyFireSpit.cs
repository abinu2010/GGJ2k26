using UnityEngine;

public class EnemyFireSpit : MonoBehaviour
{
    public Transform target;
    public Transform mouthPoint;

    public FireProjectile projectilePrefab;

    public float engageRange = 8f;
    public float cooldown = 1.2f;

    public float stopDistanceFromPlayer = 4.5f;
    public float stopTolerance = 0.25f;

    public float projectileSpeed = 10f;
    public float projectileDamage = 12f;
    public float projectileLifeTime = 2.5f;

    public LayerMask projectileHitLayers;

    public bool logShooting = false;

    float nextShotTime;

    void Update()
    {
        if (target == null) return;
        if (projectilePrefab == null) return;
        if (Time.time < nextShotTime) return;

        float posX = transform.position.x;
        float targetPosX = target.position.x;

        float horizontalDistanceToTarget = targetPosX - posX;
        float absHorizontalDistanceToTarget = Mathf.Abs(horizontalDistanceToTarget);

        if (absHorizontalDistanceToTarget > engageRange)
        {
            if (logShooting) Debug.Log(name + " not shooting: outside engageRange, dist=" + absHorizontalDistanceToTarget);
            return;
        }

        SpawnProjectile(horizontalDistanceToTarget);
        nextShotTime = Time.time + cooldown;

        if (logShooting) Debug.Log(name + " shot fired, dist=" + absHorizontalDistanceToTarget);
    }

    void SpawnProjectile(float horizontalDistanceToTarget)
    {
        float spawnPosX = mouthPoint != null ? mouthPoint.position.x : transform.position.x;
        float spawnPosY = mouthPoint != null ? mouthPoint.position.y : transform.position.y;
        float spawnPosZ = mouthPoint != null ? mouthPoint.position.z : transform.position.z;

        FireProjectile projectile = Instantiate(projectilePrefab, new Vector3(spawnPosX, spawnPosY, spawnPosZ), Quaternion.identity);
        projectile.speed = projectileSpeed;
        projectile.damage = projectileDamage;
        projectile.lifeTime = projectileLifeTime;
        projectile.hitLayers = projectileHitLayers;
        projectile.SetDirection(horizontalDistanceToTarget);
    }

    void OnDrawGizmosSelected()
    {
        float posX = transform.position.x;
        float posY = transform.position.y;
        float posZ = transform.position.z;

        Vector3 origin = new Vector3(posX, posY, posZ);

        Gizmos.color = new Color(1f, 0.9f, 0.2f, 1f);
        DrawHorizontalRange(origin, engageRange);

        float minStopDistance = Mathf.Max(0f, stopDistanceFromPlayer - stopTolerance);
        float maxStopDistance = Mathf.Max(minStopDistance, stopDistanceFromPlayer + stopTolerance);

        Gizmos.color = new Color(1f, 0.25f, 0.25f, 1f);
        DrawHorizontalRange(origin, minStopDistance);

        Gizmos.color = new Color(0.2f, 0.9f, 1f, 1f);
        DrawHorizontalRange(origin, maxStopDistance);
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
