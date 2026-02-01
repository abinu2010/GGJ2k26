using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 6f;
    public float jumpForce = 7f;
    public float attackAnimationDuration = 0.5f; // Set this in the Inspector based on your animation length

    Rigidbody rb;
    public Animator animator;

    bool facingRight;
    bool isGrounded;

    public WeaponManager weaponManager;
    public Camera mainCamera;
    AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        facingRight = true;
    }

    void Update()
    {
        // Footsteps
        if (isGrounded && Mathf.Abs(rb.linearVelocity.x) > 0.1f)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = SoundManager.Instance.footsteps;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying && audioSource.clip == SoundManager.Instance.footsteps)
            {
                audioSource.Stop();
            }
        }

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetBool("isJumping", true);
            isGrounded = false;
        }

        // Floating
        if (!isGrounded && rb.linearVelocity.y < 0)
        {
            animator.SetBool("isFloating", true);
        }
        else
        {
            animator.SetBool("isFloating", false);
        }


        if (Input.GetMouseButtonDown(0))
        {
            if (weaponManager == null) return;
            if (weaponManager.currentWeapon == null) return;
            if (mainCamera == null) return;

            Weapon weapon = weaponManager.currentWeapon.GetComponentInChildren<Weapon>(true);
            if (weapon == null)
            {
                Debug.Log("No Weapon component found under currentWeapon: " + weaponManager.currentWeapon.name);
                return;
            }

            animator.SetTrigger("Attack");

            if (weapon.GetComponent<Sword>() != null)
            {
                StartCoroutine(AttackCoroutine("isSwordAttacking"));
            }
            else if (weapon.GetComponent<Bow>() != null)
            {
                StartCoroutine(AttackCoroutine("isBowAttacking"));
            }
            else if (weapon.GetComponent<Wand>() != null)
            {
                StartCoroutine(AttackCoroutine("isWandAttacking"));
            }


            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane gamePlane = new Plane(Vector3.forward, transform.position);
            float distance;

            if (gamePlane.Raycast(ray, out distance))
            {
                Vector3 worldMousePosition = ray.GetPoint(distance);
                weapon.Attack(worldMousePosition);
            }
        }
    }

    IEnumerator AttackCoroutine(string attackBoolName)
    {
        animator.SetBool(attackBoolName, true);
        yield return new WaitForSeconds(attackAnimationDuration);
        animator.SetBool(attackBoolName, false);
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
        animator.SetBool("isJumping", false);
        animator.SetBool("isLanding", true);
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
        animator.SetBool("isLanding", false);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 currentScale = transform.localScale;
        transform.localScale = new Vector3(currentScale.x * -1f, currentScale.y, currentScale.z);
    }
}
