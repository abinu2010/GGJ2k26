using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 25f;

    public virtual void Attack(Vector3 targetPosition)
    {
    }
}
