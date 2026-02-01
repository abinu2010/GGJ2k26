using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public float fullHealth;
    float currentHealth;

    public Transform dmgPoint;
    public GameObject blood;

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
        Debug.Log(gameObject.name + " took " + damage + " damage. Current health: " + currentHealth);

        //blood effect
        if (blood != null)
        {
            Vector3 pos = dmgPoint != null ? dmgPoint.position : transform.position;
            Quaternion rot = dmgPoint != null ? dmgPoint.rotation : Quaternion.identity;
            GameObject fx = Instantiate(blood, pos, rot);
            Destroy(fx, 2f);
        }

        if (currentHealth <= 0)
        {
            SceneChange();  
            isDead();
        }
    }
    public void isDead()
    {
        
        Destroy(gameObject);
    }

    public void SceneChange()
    {
        SceneManager.LoadScene("MainMenu");
    }    
}
