using System.Collections.Generic;
using UnityEngine;

public class PrefabPooler : MonoBehaviour
{
    [System.Serializable]
    public struct PoolItem
    {
        public GameObject prefab;
        public int poolSize;
    }
    public PoolItem[] poolItems;
    private Dictionary<GameObject, Queue<GameObject>> pools;
    void Awake()
    {
        pools = new Dictionary<GameObject, Queue<GameObject>>();
        foreach (var item in poolItems)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();
            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject obj = CreateNewObject(item.prefab);
                objectQueue.Enqueue(obj);
            }
            pools.Add(item.prefab, objectQueue);
        }
    }
    public GameObject GetPooledObject(GameObject prefab)
    {
        if (pools.TryGetValue(prefab, out Queue<GameObject> poolQueue))
        {
            if (poolQueue.Count > 0)
            {
                GameObject obj = poolQueue.Dequeue();
                poolQueue.Enqueue(obj);
                return obj;
            }
            else
            {
                GameObject obj = CreateNewObject(prefab);
                poolQueue.Enqueue(obj);
                return obj;
            }
        }
        else
        {
            print($"{prefab.name} pool could not be found.");
            return null;
        }
    }
    GameObject CreateNewObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        return obj;
    }
    //public void ReturnPooledObject(GameObject prefab,GameObject prefabInstance)
    //{
    //    if (pools.TryGetValue(prefab, out Queue<GameObject> poolQueue))
    //    {
    //        prefabInstance.SetActive(false);
    //        poolQueue.Enqueue(prefabInstance);
    //    }
    //    else
    //    {
    //        print($"{prefab.name} pool could not be found.");
    //    }
    //}
}
