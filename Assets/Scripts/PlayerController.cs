using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D standBoxCollider;
    [SerializeField] private BoxCollider2D crouchBoxCollider;
    private bool jumpRequested;
    private bool crouchRequested;
    private float initialPlayerHeight;
    private Rigidbody2D rigidBody2d;
    public float jumpHeight;
    void Start()
    {
        initialPlayerHeight = transform.position.y;
        rigidBody2d = transform.GetComponent<Rigidbody2D>();
        crouchBoxCollider.enabled = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))
        {
          jumpRequested = true;
        }
        if (Input.GetKey(KeyCode.DownArrow) || (Input.GetKey(KeyCode.S)))
        {
            crouchRequested = true;
        }

    }
    private void FixedUpdate()
    {
        HandleJump();
        HandleCrouch();

    }
    void HandleCrouch()
    {
        if (crouchRequested)
        {
            crouchBoxCollider.enabled = true;
            standBoxCollider.enabled = false;
            crouchRequested = false;
        }
        else
        {
            standBoxCollider.enabled = true;
            crouchBoxCollider.enabled = false;
        }
    }
    void HandleJump()
    {
        if (Mathf.Abs(transform.position.y - initialPlayerHeight) < 0.1f && jumpRequested)
        {
            rigidBody2d.linearVelocity = Vector2.up * jumpHeight;
        }
        jumpRequested = false;

    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Obstacle"))
        {
            print("GAME OVER");
        }
    }
}
