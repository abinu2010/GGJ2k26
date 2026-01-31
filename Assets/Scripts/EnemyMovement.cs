using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public Rigidbody rb;

    public float speed = 3f;
    public float stopDistance = 1.2f;

    float fixedPosZ;

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        fixedPosZ = transform.position.z;
    }

    void FixedUpdate()
    {
        if (target == null || rb == null) return;

        float posX = transform.position.x;
        float posY = transform.position.y;
        float posZ = fixedPosZ;

        transform.position = new Vector3(posX, posY, posZ);

        float targetPosX = target.position.x;
        float distanceX = targetPosX - posX;
        float absDistanceX = Mathf.Abs(distanceX);

        if (absDistanceX <= stopDistance)
        {
            float velY = rb.linearVelocity.y;
            rb.linearVelocity = new Vector3(0f, velY, 0f);
            return;
        }

        float moveDirX = Mathf.Sign(distanceX);
        float velY2 = rb.linearVelocity.y;
        rb.linearVelocity = new Vector3(moveDirX * speed, velY2, 0f);

        Vector3 scale = transform.localScale;
        if (moveDirX > 0f && scale.x < 0f) transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        if (moveDirX < 0f && scale.x > 0f) transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
    }
}
