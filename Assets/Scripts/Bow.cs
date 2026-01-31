using UnityEngine;

public class Bow : Weapon
{
    public GameObject arrowPrefab;
    public Transform firePoint;

    public override void Attack()
    {
        if (arrowPrefab != null && firePoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position + firePoint.right * 1.5f, firePoint.rotation * Quaternion.Euler(0, 0, 90));
            Projectile projectile = arrow.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.SetDirection(firePoint.right);
            }
        }
    }
}
