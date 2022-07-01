using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPooler : PoolBase
{
    public static ObjectPooler instance;
    private void Awake() {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        InitPool();
    }
    public GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation) =>
        _SpawnFromPool(prefab, position, rotation);
 
    protected override GameObject CreateNewObject(string name, GameObject prefab)
    {
        var obj = Instantiate(prefab, transform);
        obj.name = name;
        obj.SetActive(false); // call OnDisable() --> ReturnToPool

        return obj;
    }

    protected override GameObject _SpawnFromPool(string name, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(name))
            throw new Exception($"Pool with name {name} doesn't exist");

        // if Queue is empty(any deactivated object exist) create new
        Queue<GameObject> poolQueue = poolDictionary[name];
        if (poolQueue.Count <= 0)
        {
            Pool pool = Array.Find(pools, x => x.name == name);
            var obj = CreateNewObject(pool.name, pool.prefab);
            ArrangePool(obj); // insert into pool
        }

        // activate object from pool
        GameObject objectToSpawn = poolQueue.Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    GameObject _SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(prefab.name))
        {
            Debug.LogWarning("Pool with name " + prefab.name + "doesn't exist // Instantiate object");
            return Instantiate(prefab, position, rotation);
        }

        return _SpawnFromPool(prefab.name, position, rotation);
    }
}