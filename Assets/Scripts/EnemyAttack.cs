using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public Transform target;
    public Transform hitPoint;
    public LayerMask playerLayer;

    public float engageRange = 1.5f;
    public float hitRadius = 0.6f;

    public float holdDistanceFromPlayer = 1.4f;
    public float holdTolerance = 0.25f;

    public float damage = 10f;
    public float cooldown = 1.2f;

    public float attackAnimationDuration = 0.5f;
    public float hitDelay = 0.2f;

    float nextAttackTime;
    Animator animator;
    bool attacking;

    void Awake()
    {
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) target = playerObj.transform;
        }

        animator = GetComponentInChildren<Animator>(true);
    }

    void Update()
    {
        if (target == null) return;
        if (attacking) return;
        if (Time.time < nextAttackTime) return;

        float absHorizontalDistanceToTarget = Mathf.Abs(target.position.x - transform.position.x);
        if (absHorizontalDistanceToTarget > engageRange) return;

        nextAttackTime = Time.time + cooldown;
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        attacking = true;

        if (animator != null)
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("attack");
            animator.SetBool("isAttacking", true);
        }

        yield return new WaitForSeconds(hitDelay);
        Hit();

        float remaining = Mathf.Max(0f, attackAnimationDuration - hitDelay);
        yield return new WaitForSeconds(remaining);

        if (animator != null) animator.SetBool("isAttacking", false);
        attacking = false;
    }

    public void Hit()
    {
        float posX = hitPoint != null ? hitPoint.position.x : transform.position.x;
        float posY = hitPoint != null ? hitPoint.position.y : transform.position.y;
        float posZ = hitPoint != null ? hitPoint.position.z : transform.position.z;

        Vector3 hitPosition = new Vector3(posX, posY, posZ);

        Collider[] hitColliders = Physics.OverlapSphere(hitPosition, Mathf.Max(0f, hitRadius), playerLayer);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i] == null) continue;

            Health health = hitColliders[i].GetComponentInParent<Health>();
            if (health != null)
            {
                health.addDamage(damage);
                return;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        float posX = hitPoint != null ? hitPoint.position.x : transform.position.x;
        float posY = hitPoint != null ? hitPoint.position.y : transform.position.y;
        float posZ = hitPoint != null ? hitPoint.position.z : transform.position.z;

        Gizmos.DrawWireSphere(new Vector3(posX, posY, posZ), Mathf.Max(0f, hitRadius));
    }
}
