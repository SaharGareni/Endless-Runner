using UnityEngine;

public class TileScript : MonoBehaviour
{
    private float screenHalfWidth;
    public static float speed = 5f;
    void Start()
    {
        screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector2.left *Time.deltaTime * speed);
        if (transform.position.x <= -screenHalfWidth - transform.localScale.x/2)
        {
            gameObject.SetActive(false);
        }
    }
}
