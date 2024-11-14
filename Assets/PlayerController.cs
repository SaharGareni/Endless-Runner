using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float initialPlayerHeight;
    private float playerAngle;
    public float jumpHeight;
    public float crouchAngle = 45f;
    void Start()
    {
        initialPlayerHeight = transform.position.y;
    }

    void Update()
    {
        HandleJump();
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
               transform.position = new Vector2 (transform.position.x, transform.position.y + jumpHeight);
            }
        }
        float targetPlayerHeight =Mathf.Lerp(initialPlayerHeight,initialPlayerHeight+jumpHeight,Time.deltaTime);
        transform.position = new Vector2(transform.position.x, transform.position.y + targetPlayerHeight);
    }
}
