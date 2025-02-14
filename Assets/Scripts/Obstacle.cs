using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Vector2 minMaxSpeed;
    [SerializeField] private float spawnHeight;
    [SerializeField] private bool useRandomHeight;
    [SerializeField] private float heightVariation;
    //private float speed;
    private float despawnRange;
    void Start()
    {
        despawnRange = -(Camera.main.aspect * Camera.main.orthographicSize) - Mathf.Abs(transform.localPosition.x / 2);
        //speed = TileScript.speed;
    }
    void FixedUpdate()
    {
        //TODO: Change the static refrence of GameManager to the static instance of it instead (singleton)
        float speed = Mathf.Lerp(minMaxSpeed.x,minMaxSpeed.y, GameManager.GetDifficultyPercentage());
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        if (transform.position.x < despawnRange)
        {
            gameObject.SetActive(false);
        }
    }
    public float GetSpawnHeight()
    {
        if (useRandomHeight)
        {
            return spawnHeight + UnityEngine.Random.Range(-heightVariation, heightVariation);
        }
        return spawnHeight;
    }
}
