using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D standBoxCollider;
    [SerializeField] private BoxCollider2D crouchBoxCollider;
    private bool jumpRequested;
    private bool crouchRequested;
    private bool isGrounded;
    private bool isCrouching;
    private float initialPlayerHeight;
    private Animator animator;
    private Rigidbody2D rigidBody2d;
    public float jumpHeight;
    public static event System.Action OnPlayerDeath;
    void Start()
    {
        //Time.timeScale = 0.1f; // for debugging purposes
        initialPlayerHeight = transform.position.y;
        rigidBody2d = transform.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        crouchBoxCollider.enabled = false;
        isCrouching = crouchBoxCollider.enabled;
        isGrounded = true;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.LeftAlt))
        {
          jumpRequested = true;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || (Input.GetKey(KeyCode.S)))
        {
          crouchRequested = true;
        }

    }
    private void FixedUpdate()
    {
        isGrounded = Mathf.Abs(transform.position.y - initialPlayerHeight) < 0.1f;
        HandleJump();
        HandleCrouch();
        HandleAnimations();
    }
    void HandleCrouch()
    {
        if (crouchRequested)
        {
            crouchBoxCollider.enabled = true;
            standBoxCollider.enabled = false;
        }
        else
        {
            standBoxCollider.enabled = true;
            crouchBoxCollider.enabled = false;
        }
        crouchRequested = false;
        isCrouching = crouchBoxCollider.enabled;
    }
    void HandleJump()
    {
        if (isGrounded && jumpRequested)
        {
            rigidBody2d.linearVelocity = Vector2.up * jumpHeight;
        }
        jumpRequested = false;

    }
    void HandleAnimations()
    {
        animator.SetBool("isJumping", !isGrounded);
        animator.SetFloat("yVelocity",rigidBody2d.linearVelocityY);
        animator.SetBool("isCrouching", isCrouching);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.CompareTag("Obstacle"))
        {
            animator.SetTrigger("Hit");
            OnPlayerDeath?.Invoke();
        }
    }
}
