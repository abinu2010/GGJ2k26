using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 25f;
    public float attackCooldown = 0.5f;
    public float nextAttackTime = 0;

    public virtual float GetDamage()
    {
        return damage;
    }

    public virtual void Attack(Vector3 targetPosition)
    {
        if(Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
        }
    }
}
