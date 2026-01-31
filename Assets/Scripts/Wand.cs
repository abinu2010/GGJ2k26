using UnityEngine;

public class Wand : Weapon
{
    public GameObject fireballPrefab;
    public Transform firePoint;

    public override void Attack()
    {
        if (fireballPrefab != null && firePoint != null)
        {
            GameObject fireball = Instantiate(fireballPrefab, firePoint.position + firePoint.right * 5f, firePoint.rotation);
            Projectile projectile = fireball.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.SetDirection(firePoint.right);
            }
        }
    }
}
