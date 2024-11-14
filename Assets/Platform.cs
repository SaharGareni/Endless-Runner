using UnityEngine;

public class Platform : MonoBehaviour
{
    private float screenHalfWidth;
    private float platformWidth;
    public float speed = 5f;
    public Transform firstSegment;
    public Transform secondSegment;

    void Start()
    {
        screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
        platformWidth = firstSegment.transform.localScale.x;
    }

    void Update()
    {
        if (firstSegment != null && secondSegment != null)
        {
            firstSegment.position += Vector3.left * Time.deltaTime * speed;
            secondSegment.position += Vector3.left * Time.deltaTime * speed;

            if (firstSegment.position.x + platformWidth/2f < -screenHalfWidth)
            {
                firstSegment.position = new Vector3(secondSegment.position.x+platformWidth, secondSegment.position.y, secondSegment.position.z);
            }
            if (secondSegment.position.x + 15.55f < -screenHalfWidth)
            {
                secondSegment.position = new Vector3(firstSegment.position.x + platformWidth, firstSegment.position.y, firstSegment.position.z);
            }
        }
    }
}
