using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private float spawnHorizontalOffset = 2f;
    public GameObject player;
    public ObjectPooler obstaclePool;
    public float spawnInterval = 1.0f;
    private float playerHeight;
    private float playerYPosition;
    private float mainCameraHalfWidth;
    private float horizontalSpawnPosition;
    void Start()
    {
        playerHeight = player.GetComponent<BoxCollider2D>().bounds.size.y;
        playerYPosition = player.transform.position.y;
        mainCameraHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
        horizontalSpawnPosition = spawnHorizontalOffset + mainCameraHalfWidth;
        InvokeRepeating("SpawnObstacle", 0f, spawnInterval);
    }
    void SpawnObstacle()
    {
        float obstacleGroundPosition = playerYPosition;
        float obstacleSkyPosition = playerYPosition + playerHeight*0.75f;
        float verticalSpawnPosition = Random.Range(0, 2) == 0 ? obstacleGroundPosition : obstacleSkyPosition;
        GameObject obstacle = obstaclePool.GetPooledObject();
        obstacle.transform.position = new Vector2(horizontalSpawnPosition, verticalSpawnPosition);
        obstacle.SetActive(true);
    }
}
