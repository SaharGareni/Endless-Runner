using UnityEngine;

public class TileScript : MonoBehaviour
{
    private float screenHalfWidth;
    public static float speed = 5f;
    void Start()
    {
        screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
    }
    void FixedUpdate()
    {
        transform.Translate(Vector2.left *Time.deltaTime * speed);
        //for single tiles
        //if (transform.position.x <= -screenHalfWidth - transform.localScale.x/2)
        //{
        //    gameObject.SetActive(false);
        //}
        if (transform.position.x < -screenHalfWidth * 3)
        {
            gameObject.SetActive(false);
        }
    }
}
