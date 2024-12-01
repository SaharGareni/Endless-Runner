using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private PrefabPooler prefabPooler;
    [SerializeField] private float spawnHorizontalOffset = 1f;
    public GameObject player;
    public float spawnInterval = 1f;
    private float playerHeight;
    private float playerYPosition;
    private float mainCameraHalfWidth;
    private float horizontalSpawnPosition;
    private Vector3 spawnLocation;
    void Start()
    {
        playerHeight = player.GetComponent<BoxCollider2D>().bounds.size.y;
        playerYPosition = player.transform.position.y;
        mainCameraHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
        horizontalSpawnPosition = spawnHorizontalOffset + mainCameraHalfWidth;
        spawnLocation.x = horizontalSpawnPosition;
    }
    IEnumerator SpawnObstacle()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, prefabPooler.poolItems.Length);
            GameObject randomPrefab = prefabPooler.poolItems[randomIndex].prefab;
            string objTag = randomPrefab.tag;
            switch (objTag)
            {
                case "GroundObstacle":
                    spawnLocation.y = playerYPosition;
                    break;
                case "LowObstacle":
                   spawnLocation.y = playerYPosition + 0.85f;
                    break;
                case "HighObstacle":
                    spawnLocation.y = playerYPosition + 0.95f;
                    break;
                default:
                    spawnLocation.y = playerYPosition;
                    break;

            }
            prefabPooler.GetPooledObject(randomPrefab, spawnLocation);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    void SpawnRandomObstacle()
    {

    }
}
