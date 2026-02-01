using UnityEngine;

public class Bow : Weapon
{
    public GameObject arrowPrefab;
    public Transform firePoint;

    public override float GetDamage()
    {
        if (GameManager.Instance == null)
        {
            return damage;
        }
        return damage + GameManager.Instance.bowDamageBonus;
    }

    public override void Attack(Vector3 targetPosition)
    {
        if(Time.time < nextAttackTime)
        {
            return;
        }
        base.Attack(targetPosition);
        if (arrowPrefab != null && firePoint != null)
        {
            Vector3 direction = (targetPosition - firePoint.position).normalized;
            
            // We don't want to change the z-axis, as this is a 2d game
            direction.z = 0;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position + direction, Quaternion.identity);
            Projectile projectile = arrow.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.damage = GetDamage();
                projectile.SetDirection(direction);
            }
        }
    }
}
