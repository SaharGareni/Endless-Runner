using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] private PrefabPooler prefabPooler;
    [SerializeField] private GameObject defaultTilePrefab;
    private float defaultPrefabHalfScale;
    private Vector3 spawnLocation;
    private float spawnInterval;
    private float nextSpawnTime;
    //screen width/speed for spawning when a prefab is out of camera
    //when calcing spawn location if using large prefab can skip using defaultPrefabHalfScale since pivot is on the left side
    //tile length/tile speed for seamless placement.
    void Start()
    {
        defaultPrefabHalfScale = defaultTilePrefab.transform.localScale.x/2;
        spawnLocation = new Vector3(Camera.main.aspect * Camera.main.orthographicSize, -Camera.main.orthographicSize);
        //for single tiles - spawn location should be: the prefab length + half the camera width
        //spawnLocation = new Vector3(Camera.main.aspect * Camera.main.orthographicSize + defaultPrefabHalfScale, -Camera.main.orthographicSize + defaultPrefabHalfScale);
        //for single tiles - spawning interval should be: the length of the object / speed (unit per second)
        //this is to individually spawn the tiles without gaps as the "spawnInterval" may dictate a gap.
        //spawnInterval = defaultPrefabHalfScale*2/TileScript.speed - 0.01f;
        spawnInterval = (Camera.main.aspect * Camera.main.orthographicSize * 2) / TileScript.speed;
        StartCoroutine("TileSpawnCoroutine");

    }

    void FixedUpdate()
    {
        //spawning implementation without coroutine 
        //if (Time.time >= nextSpawnTime)
        //{
        //    GameObject obj = prefabPooler.GetPooledObject(defaultTilePrefab);
        //    obj.transform.position = spawnLocation;
        //    obj.SetActive(true);
        //    nextSpawnTime += spawnInterval;
        //}
        //lag machine to check for bugs
        //for (int i = 0; i < 1000000; i++)
        //{
        //    float temp = Mathf.Atan2(i, 5);
        //}

    }
    IEnumerator TileSpawnCoroutine()
    {
        while (true) 
        {
            int randomIndex = Random.Range(0,prefabPooler.poolItems.Length);
            GameObject obj = prefabPooler.GetPooledObject(prefabPooler.poolItems[randomIndex].prefab);
            obj.transform.position = spawnLocation;
            obj.SetActive(true);
            yield return new WaitForSeconds(spawnInterval);
        } 
        
    }
}
