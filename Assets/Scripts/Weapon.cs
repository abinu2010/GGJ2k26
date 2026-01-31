using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 25f;

    public virtual void Attack()
    {
        // Default attack behavior (can be empty)
    }
}
