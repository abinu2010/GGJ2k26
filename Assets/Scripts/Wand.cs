using UnityEngine;

public class Wand : Weapon
{
    public GameObject fireballPrefab;
    public Transform firePoint;

    public override float GetDamage()
    {
        if (GameManager.Instance == null)
        {
            return damage;
        }
        return damage + GameManager.Instance.wandDamageBonus;
    }

    public override void Attack(Vector3 targetPosition)
    {
        if (fireballPrefab != null && firePoint != null)
        {
            Vector3 direction = (targetPosition - firePoint.position).normalized;

            direction.z = 0;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            GameObject fireball = Instantiate(fireballPrefab, firePoint.position + direction, rotation);
            Projectile projectile = fireball.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.damage = GetDamage();
                projectile.SetDirection(direction);
            }
        }
    }
}
