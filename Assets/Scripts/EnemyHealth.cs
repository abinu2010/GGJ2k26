using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 30f;
    public float currentHealth = 30f;
    public bool dead;

    public bool logDamage = true;

    public Transform dmgPoint;
    public GameObject blood;

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

        //blood effect
        if (blood != null)
        {
            Vector3 pos = dmgPoint != null ? dmgPoint.position : transform.position;
            Quaternion rot = dmgPoint != null ? dmgPoint.rotation : Quaternion.identity;
            GameObject fx = Instantiate(blood, pos, rot);
            Destroy(fx, 2f);
        }

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
            if (maskPieceDrop != null)
            {
                maskPieceDrop.Drop();
            }

            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {

    }
}
