using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 25f;

    public virtual float GetDamage()
    {
        return damage;
    }

    public virtual void Attack(Vector3 targetPosition)
    {
    }
}
