using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 30f;
    public float currentHealth = 30f;
    public bool dead;

    MaskPieceDrop maskPieceDrop;

    void Awake()
    {
        currentHealth = maxHealth;
        maskPieceDrop = GetComponent<MaskPieceDrop>();
    }

    public void AddDamage(float damage)
    {
        if (dead) return;

        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            dead = true;

            if (maskPieceDrop != null) maskPieceDrop.Drop();

            Destroy(gameObject);
        }
    }
}
