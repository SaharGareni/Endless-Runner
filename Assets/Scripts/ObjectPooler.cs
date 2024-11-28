using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private int poolSize = 10;
    public GameObject obstaclePrefab;
    private Queue<GameObject> pool;
    void Start()
    {
        InitializePool();
    }
    GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(obstaclePrefab,transform);
        obj.SetActive(false);
        return obj;
    }
    void InitializePool()
    {
        pool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            pool.Enqueue(CreateNewObject());
        }
    }
    public GameObject GetPooledObject()
    {
        if (pool.Count > 0)
        {

            GameObject obj = pool.Dequeue();
            return obj;
            
        }
        else
        {
            return CreateNewObject();
        }
    }
    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

}
