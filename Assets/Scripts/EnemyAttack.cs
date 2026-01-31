using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform target;
    public Transform hitPoint;
    public LayerMask playerLayer;

    public float engageRange = 1.0f;
    public float hitRadius = 0.6f;

    public float damage = 10f;
    public float cooldown = 1.2f;

    float nextTime;

    void Update()
    {
        if (target == null) return;
        if (Time.time < nextTime) return;

        float posX = transform.position.x;
        float targetPosX = target.position.x;

        float distanceX = targetPosX - posX;
        float absDistanceX = Mathf.Abs(distanceX);

        if (absDistanceX > engageRange) return;

        Hit();
        nextTime = Time.time + cooldown;
    }

    void Hit()
    {
        float hitPosX = hitPoint != null ? hitPoint.position.x : transform.position.x;
        float hitPosY = hitPoint != null ? hitPoint.position.y : transform.position.y;
        float hitPosZ = hitPoint != null ? hitPoint.position.z : transform.position.z;

        Vector3 hitPos = new Vector3(hitPosX, hitPosY, hitPosZ);

        Collider[] hits = Physics.OverlapSphere(hitPos, hitRadius, playerLayer);

        for (int i = 0; i < hits.Length; i++)
        {
            PlayerHealth playerHealth = hits[i].GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.addDamage(damage);
                return;
            }
        }
    }
}
