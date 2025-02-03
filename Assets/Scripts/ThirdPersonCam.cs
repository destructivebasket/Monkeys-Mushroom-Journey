using System.Collections;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Vector3 playerVelocity;
    public float speed = 6f;
    public float dashSpeed = 20f;
    public float rotationSpeed = 720f;
    public float turnSmoothTime = 0.1f;
    public float dashDuration = 0.2f;
    public float jumpForce = 1.5f;
    public float gravity = -9.81f;

    private Animator animator;
    private bool isDashing = false;
    private bool isGrounded = true;

    /*  public enum CameraStyle
      {
          Basic, Combat, Topdown
      };

      public CameraStyle currentStyle;

      public GameObject basicCam;*/
    //public GameObject combatCam;

    public HealthBar health;
    public int maxHealth = 3;
    public int currentHealth;
    private bool isDead = false;

    private Rigidbody rb;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        health.SetMaxHealth(maxHealth);

        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        if (!isDead)
        {
            Movement();
        }
        else
        {
            controller.Move(new Vector3(0, 0, 0));
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            TakeDamage(1);
        }

    }

    /*private void OnCollisionExit(Collision other)
    {
        if(other.gameObject.tag == "Enemy")
        {  
            TakeDamage(1);
        }
    }*/


    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        health.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            animator.SetTrigger("Death");
            isDead = true;
        }
        else
        {
            animator.SetTrigger("Hit");
        }


    }

    /*private void switchCamera(CameraStyle newStyle)
    {
        combatCam.SetActive(false);
        basicCam.SetActive(false);

        if(newStyle == CameraStyle.Basic) basicCam.SetActive(true);
        if(newStyle == CameraStyle.Combat) combatCam.SetActive(true);


        currentStyle = newStyle;
    }*/

    public void Movement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = gravity * Time.fixedDeltaTime;
        }

        if (playerVelocity.y < 1f && playerVelocity.x < 0.1f && playerVelocity.z < 0.1f && isGrounded)
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Run", false);
        }
        else
        {
            animator.SetBool("Idle", false);
        }

        float horizontalDir = Input.GetAxisRaw("Horizontal");
        float verticalDir = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontalDir, 0, verticalDir).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (!isDashing)
            {
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }

            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Run", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpForce * -2f * gravity);
            animator.SetTrigger("Jump");
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
            playerVelocity /= 2;
        }


    }

    private IEnumerator Dash()
    {
        isDashing = true;
        animator.SetTrigger("Roll");
        Vector3 dashDirection = transform.forward; // Dash in the direction the character is facing

        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
    }
}
