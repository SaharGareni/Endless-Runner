using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private float speed;
    private float spriteLength;
    private float screenHalfWidth;

    void Start()
    {
        spriteLength = GetComponent<SpriteRenderer>().sprite.bounds.size.x * transform.localScale.x;
        screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
    }
    void FixedUpdate()
    {
        transform.Translate(Vector2.left * speed *Time.deltaTime);
        if (transform.position.x <= -spriteLength + 0.2f)
        {
            transform.position = new Vector2(spriteLength, transform.position.y);  
        }

    }
}
