using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 30f;
    public float currentHealth = 30f;
    public bool dead;

    public bool logDamage = true;

    MaskPieceDrop maskPieceDrop;

    void Awake()
    {
        currentHealth = maxHealth;
        maskPieceDrop = GetComponent<MaskPieceDrop>();

        if (logDamage)
        {
            Debug.Log(name + " spawned with HP: " + currentHealth + " / " + maxHealth);
        }
    }

    public void AddDamage(float damage)
    {
        if (dead) return;

        float healthBefore = currentHealth;

        currentHealth -= damage;
        if (currentHealth < 0f) currentHealth = 0f;

        if (logDamage)
        {
            Debug.Log(name + " took damage: " + damage + " | HP: " + healthBefore + " -> " + currentHealth + " / " + maxHealth);
        }

        if (currentHealth <= 0f)
        {
            dead = true;

            if (logDamage)
            {
                Debug.Log(name + " died");
            }

            if (KnowledgeManager.Instance != null && !GameManager.Instance.knowledgeCheckPerformed)
            {
                KnowledgeManager.Instance.ShowKnowledgeCheck(maskPieceDrop, gameObject);
                gameObject.SetActive(false); // Disable the enemy instead of destroying it
            }
            else
            {
                if (maskPieceDrop != null) maskPieceDrop.Drop();
                Destroy(gameObject);
            }
        }
    }
}
