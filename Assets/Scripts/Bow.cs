using UnityEngine;

public class Bow : Weapon
{
    public GameObject arrowPrefab;
    public Transform firePoint;

    public override void Attack(Vector3 targetPosition)
    {
        if (arrowPrefab != null && firePoint != null)
        {
            Vector3 direction = (targetPosition - firePoint.position).normalized;
            
            // We don't want to change the z-axis, as this is a 2d game
            direction.z = 0;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, rotation);
            Projectile projectile = arrow.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.SetDirection(direction);
            }
        }
    }
}
