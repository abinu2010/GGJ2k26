using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 6f;
    public float jumpForce = 7f;
    Rigidbody rb;
    Animator animator;
    bool facingRight;
    bool isGrounded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        facingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Vector3 currentLinearVelocity = rb.linearVelocity;
            rb.linearVelocity = new Vector3(currentLinearVelocity.x, jumpForce, currentLinearVelocity.z);
            isGrounded = false;
        }
    }
    void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");

        if (animator != null)
        {
            animator.SetFloat("speed", Mathf.Abs(move));
        }
        Vector3 currentLinearVelocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(move * runSpeed, currentLinearVelocity.y, currentLinearVelocity.z);
        if (move > 0f && !facingRight) Flip();
        else if (move < 0f && facingRight) Flip();
    }
    void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 currentScale = transform.localScale;
        transform.localScale = new Vector3(currentScale.x * -1f, currentScale.y, currentScale.z);
    }
}
