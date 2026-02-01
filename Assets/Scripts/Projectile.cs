using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 2f;
    public float damage = 10f;

    private Rigidbody rb;
    private Vector3 _direction;

    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
        _direction.z = 0; // Ensure no Z-axis movement
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = _direction * speed;
            rb.freezeRotation = true;
        }
        else
        {
            Debug.LogError("Rigidbody component not found on the projectile!"); // Keep this error for critical missing component
        }
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject.name + " collided with " + other.name);
        EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
        if (enemyHealth != null)
        {
            Debug.Log(gameObject.name + " is applying " + damage + " damage to " + other.name);
            enemyHealth.AddDamage(damage);
        }
        else
        {
            Debug.Log("No EnemyHealth component found on " + other.name + " or its parents.");
        }
        Destroy(gameObject);
    }
}
