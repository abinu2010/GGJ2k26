using UnityEngine;

public class EnemyFireSpit : MonoBehaviour
{
    public Transform target;
    public Transform mouthPoint;

    public FireProjectile projectilePrefab;

    public float engageRange = 7f;
    public float cooldown = 2f;

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

        float posX = transform.position.x;
        float targetPosX = target.position.x;

        float distanceX = targetPosX - posX;
        float absDistanceX = Mathf.Abs(distanceX);

        if (absDistanceX > engageRange) return;

        Spit(distanceX);
        nextTime = Time.time + cooldown;
    }

    void Spit(float directionX)
    {
        float spawnPosX = mouthPoint != null ? mouthPoint.position.x : transform.position.x;
        float spawnPosY = mouthPoint != null ? mouthPoint.position.y : transform.position.y;
        float spawnPosZ = mouthPoint != null ? mouthPoint.position.z : transform.position.z;

        FireProjectile p = Instantiate(projectilePrefab, new Vector3(spawnPosX, spawnPosY, spawnPosZ), Quaternion.identity);
        p.speed = projectileSpeed;
        p.damage = projectileDamage;
        p.lifeTime = projectileLifeTime;
        p.hitLayers = projectileHitLayers;
        p.SetDirection(directionX);
    }
}
