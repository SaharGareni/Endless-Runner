using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private Vector2 minMaxSpawnInterval;
    [SerializeField] private PrefabPooler prefabPooler;
    [SerializeField] private float spawnHorizontalOffset = 1f;
    //public float spawnInterval = 1f;
    private float mainCameraHalfWidth;
    private float horizontalSpawnPosition;
    private Vector3 spawnLocation;
    void Start()
    {
        mainCameraHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
        horizontalSpawnPosition = spawnHorizontalOffset + mainCameraHalfWidth;
        spawnLocation.x = horizontalSpawnPosition;
        StartCoroutine(SpawnObstacle());
    }
    IEnumerator SpawnObstacle()
    {
        while (true)
        {
            SpawnRandomObstacle();
            float spawnInterval = Mathf.Lerp(minMaxSpawnInterval.x, minMaxSpawnInterval.y, GameManager.GetDifficultyPercentage());
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    void SpawnRandomObstacle()
    {
        int randomIndex = Random.Range(0, prefabPooler.poolItems.Length);
        GameObject randomPrefab = prefabPooler.poolItems[randomIndex].prefab;
        spawnLocation.y = randomPrefab.GetComponent<Obstacle>().GetSpawnHeight();
        prefabPooler.GetPooledObject(randomPrefab, spawnLocation);
    }
}
