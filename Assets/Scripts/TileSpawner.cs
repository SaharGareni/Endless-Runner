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
        spawnLocation = new Vector3(Camera.main.aspect * Camera.main.orthographicSize + defaultPrefabHalfScale, -Camera.main.orthographicSize + defaultPrefabHalfScale);
        spawnInterval = /*(Camera.main.aspect * Camera.main.orthographicSize * 2)/*/1/TileScript.speed - 0.01f;
        //StartCoroutine("TileSpawnCoroutine");

    }

    void FixedUpdate()
    {
        if (Time.time >= nextSpawnTime)
        {
            GameObject obj = prefabPooler.GetPooledObject(defaultTilePrefab);
            obj.transform.position = spawnLocation;
            obj.SetActive(true);
            nextSpawnTime += spawnInterval;
        }
        //for (int i = 0; i < 1000000; i++)
        //{
        //    float temp = Mathf.Atan2(i, 5);
        //}

    }
    IEnumerator TileSpawnCoroutine()
    {
        while (true)
        {
            GameObject obj = prefabPooler.GetPooledObject(defaultTilePrefab);
            obj.transform.position = spawnLocation;
            obj.SetActive(true);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
