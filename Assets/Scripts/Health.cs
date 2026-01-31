using UnityEngine;

public class Health : MonoBehaviour
{
    public float fullHealth;
    float currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = fullHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void addDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            isDead();
        }
    }
    public void isDead()
    {
        Destroy(gameObject);
    }
       
}
