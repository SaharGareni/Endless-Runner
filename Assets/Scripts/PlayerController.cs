using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D standBoxCollider;
    [SerializeField] private BoxCollider2D crouchBoxCollider;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;
    private bool jumpRequested;
    private bool crouchRequested;
    private bool isGrounded;
    private bool isCrouching;
    private Animator animator;
    private Rigidbody2D rigidBody2d;
    public float jumpHeight;
    public static event System.Action OnPlayerDeath;
    void Start()
    {
        /*Time.timeScale = 0.1f;*/ // for debugging purposes
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
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        HandleJump();
        HandleCrouch();
        HandleAnimations();
    }
    void HandleCrouch()
    {
        if (crouchRequested && isGrounded)
        {
            crouchBoxCollider.enabled = true;
            standBoxCollider.enabled = false;
            //TODO: If you do decide to go into event invocation for sounds you can do something like this
            //if (!isCrouching) { OnJumpRequested?.Invoke() } (which will play the sound on the first the first iteration if the player holds down the key) 
            //BTW this could be a good substitute for the GameManager.CheckForInput() since the only input we have comes from the player controller. 
            if (!isCrouching)
            {
                AudioManager.instance.PlaySound("Crouch");
            }
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
            AudioManager.instance.PlaySound("Jump");
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
            AudioManager.instance.PlaySound("Hit");
            OnPlayerDeath?.Invoke();
        }
    }
}
