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
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = _direction * speed;
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
        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            health.addDamage(damage);
        }
        Destroy(gameObject);
    }
}
