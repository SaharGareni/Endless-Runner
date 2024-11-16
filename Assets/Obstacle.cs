using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 4.0f;
    private float despawnRange;
    private ObjectPooler objectPool;
    void Start()
    {
        despawnRange = -(Camera.main.aspect * Camera.main.orthographicSize) - Mathf.Abs(transform.localPosition.x / 2);
        objectPool = FindObjectOfType<ObjectPooler>();
    }
    void FixedUpdate()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        if (objectPool != null && transform.position.x < despawnRange)
        {
            objectPool.ReturnObjectToPool(gameObject);
        }
    }
}
