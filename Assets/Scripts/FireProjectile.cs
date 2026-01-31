using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 12f;
    public float lifeTime = 2.5f;

    public LayerMask hitLayers;

    float moveDirX = 1f;
    float dieTime;

    public void SetDirection(float directionX)
    {
        moveDirX = Mathf.Sign(directionX);
        if (moveDirX == 0f) moveDirX = 1f;
    }

    void Awake()
    {
        dieTime = Time.time + lifeTime;
    }

    void Update()
    {
        float posX = transform.position.x + (moveDirX * speed * Time.deltaTime);
        float posY = transform.position.y;
        float posZ = transform.position.z;
        transform.position = new Vector3(posX, posY, posZ);

        if (Time.time >= dieTime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & hitLayers.value) == 0) return;

        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.addDamage(damage);
        }

        Destroy(gameObject);
    }
}
