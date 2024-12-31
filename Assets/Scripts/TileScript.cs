
using UnityEngine;

public class TileScript : MonoBehaviour
{
    [SerializeField] private Vector2 minMaxSpeed;
    private float screenHalfWidth;
    private static float speed;
    void Start()
    {
        screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
    }
    void FixedUpdate()
    {
        speed = Mathf.Lerp(minMaxSpeed.x, minMaxSpeed.y, GameManager.GetDifficultyPercentage());
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
    //TODO: this logic is not great and i would like to replace it with  a different approach to calculate the spawn interval
    public static float GetTileSpeed()
    {
        return speed;
    }
}
