using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float initialPlayerHeight;
    private float playerAngle;
    private Rigidbody2D rigidBody2d;
    public float jumpHeight;
    public float crouchAngle = 45f;
    void Start()
    {
        initialPlayerHeight = transform.position.y;
        rigidBody2d = transform.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
          HandleJump();
        }
        HandleCrouch();
    }
    void HandleCrouch()
    {

        if (Input.GetKey(KeyCode.DownArrow))
        {
            playerAngle = crouchAngle;
        }
        else
        {
            playerAngle = 0;
        }
        transform.eulerAngles = Vector3.back * playerAngle;
    }
    void HandleJump()
    {
        if (Mathf.Abs(transform.position.y - initialPlayerHeight) < 0.1f)
        {
            rigidBody2d.linearVelocity = Vector2.up * jumpHeight;
        }

    }
}
