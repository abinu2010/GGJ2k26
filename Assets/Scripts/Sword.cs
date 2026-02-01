using UnityEngine;
using System.Collections;

public class Sword : Weapon
{
    public Collider swordCollider;
    public float attackDuration = 0.2f;

    public bool logHits = true;

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

    void OnEnable()
    {
        if (swordCollider != null) swordCollider.enabled = false;
    }

    void OnDisable()
    {
        if (swordCollider != null) swordCollider.enabled = false;
        StopAllCoroutines();
    }

    public override float GetDamage()
    {
        if (GameManager.Instance == null)
        {
            return damage;
        }
        return damage + GameManager.Instance.swordDamageBonus;
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
        if (swordCollider == null || !swordCollider.enabled) return;

        EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();

        // Only proceed if we found an EnemyHealth component
        if (enemyHealth != null)
        {
            float finalDamage = GetDamage();
            if (logHits) Debug.Log("Sword hit: " + other.name + " (" + enemyHealth.name + ") with damage=" + finalDamage);
            enemyHealth.AddDamage(finalDamage);
        }
    }
}
