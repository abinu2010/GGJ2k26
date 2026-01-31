using UnityEngine;
using System.Collections; // Needed for Coroutines

public class Sword : Weapon
{
    public Collider swordCollider; // Assign this in the inspector
    public float attackDuration = 0.2f; // How long the sword stays active

    void Start()
    {
        if (swordCollider == null)
        {
            swordCollider = GetComponent<Collider>();
        }
        // Ensure the collider is initially disabled
        if (swordCollider != null)
        {
            swordCollider.enabled = false;
        }
    }

    public override void Attack()
    {
        if (swordCollider != null && !swordCollider.enabled)
        {
            StartCoroutine(SwingSword());
        }
    }

    IEnumerator SwingSword()
    {
        swordCollider.enabled = true;
        yield return new WaitForSeconds(attackDuration);
        swordCollider.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Only deal damage if the collider is currently active (i.e., during an attack swing)
        if (swordCollider != null && swordCollider.enabled)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Health health = collision.gameObject.GetComponent<Health>();
                if (health != null)
                {
                    health.addDamage(damage);
                }
            }
        }
    }
}