using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 6f;
    public float jumpForce = 7f;
    Rigidbody rb;
    Animator animator;
    bool facingRight;
    bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        facingRight = true;
    }

    public WeaponManager weaponManager;
    public Camera mainCamera;

    void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Vector3 currentLinearVelocity = rb.linearVelocity;
            rb.linearVelocity = new Vector3(currentLinearVelocity.x, jumpForce, currentLinearVelocity.z);
            isGrounded = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (weaponManager.currentWeapon != null && mainCamera != null)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                Plane gamePlane = new Plane(Vector3.forward, transform.position);
                float distance;

                if (gamePlane.Raycast(ray, out distance))
                {
                    Vector3 worldMousePosition = ray.GetPoint(distance);
                    
                    weaponManager.currentWeapon.GetComponent<Weapon>().Attack(worldMousePosition);
                }
            }
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
