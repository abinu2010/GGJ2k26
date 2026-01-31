using UnityEngine;
using System.Collections;

public class Sword : Weapon
{
    public Collider swordCollider;
    public float attackDuration = 0.2f;
    
    
    void Start()
    {
       

        if (swordCollider == null)
        {
            swordCollider = GetComponent<Collider>();
        }
        if (swordCollider != null)
        {
            swordCollider.enabled = false;
        }
    }

    public override void Attack(Vector3 targetPosition)
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