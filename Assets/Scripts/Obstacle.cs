using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed;
    private float despawnRange;
    void Start()
    {
        despawnRange = -(Camera.main.aspect * Camera.main.orthographicSize) - Mathf.Abs(transform.localPosition.x / 2);
        speed = TileScript.speed;
    }
    void FixedUpdate()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        if (transform.position.x < despawnRange)
        {
            gameObject.SetActive(false);
        }
    }
}
