using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] private PrefabPooler prefabPooler;
    private Vector3 spawnLocation;
    private float spawnInterval;

    //screen width/speed for spawning when a prefab is out of camera
    //when calcing spawn location if using large prefab can skip using defaultPrefabHalfScale since pivot is on the left side
    //tile length/tile speed for seamless placement.
    void Start()
    {
        spawnLocation = new Vector3(Camera.main.aspect * Camera.main.orthographicSize, -Camera.main.orthographicSize);
        spawnInterval = (Camera.main.aspect * Camera.main.orthographicSize * 2) / TileScript.speed;
        StartCoroutine(TileSpawnCoroutine());

    }


    IEnumerator TileSpawnCoroutine()
    {
        while (true) 
        {
            int randomIndex = Random.Range(0,prefabPooler.poolItems.Length);
            prefabPooler.GetPooledObject(prefabPooler.poolItems[randomIndex].prefab,spawnLocation);
            yield return new WaitForSeconds(spawnInterval);
        } 
        
    }
}
