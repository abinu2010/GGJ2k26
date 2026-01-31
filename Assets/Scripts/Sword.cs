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
        if (swordCollider == null) return;
        if (swordCollider.enabled) return;

        StartCoroutine(SwingSword());
    }

    IEnumerator SwingSword()
    {
        swordCollider.enabled = true;
        yield return new WaitForSeconds(attackDuration);
        swordCollider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (swordCollider == null) return;
        if (!swordCollider.enabled) return;

        EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.AddDamage(damage);
        }
    }
}
