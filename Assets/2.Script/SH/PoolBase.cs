using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class PoolBase : MonoBehaviour
{
    [Serializable]
    protected struct Pool
    {
        public string name;
        public GameObject prefab;
        public int initSize; // init size
    }

    [SerializeField] protected Pool[] pools;
    protected List<GameObject> spawnObjects;
    protected Dictionary<string, Queue<GameObject>> poolDictionary;
    protected readonly string INFO = "Pooled object must have\n"
        + "void OnDisable()\n"
        + "{\n"
        + "ObjectPooler.ReturnToPool(gameObject); // must called only \"once\" in one object\n"
        + "//CancelInvoke(); // if Script use Invoke function\n"
        + "}";
    
    [ContextMenu("GetSpawnObjectsInfo")]
    protected void GetSpawnObjectsInfo()
    {
        foreach(var pool in pools)
        {
            int count = spawnObjects.FindAll(x => x.name == pool.name).Count;
            Debug.Log($"{pool.name} count : {count}");
        }
    }

    public void InitPool()
    {
        spawnObjects = new List<GameObject>();
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            poolDictionary.Add(pool.name, new Queue<GameObject>());

            for (int i = 0; i < pool.initSize; i++)
            {
                var obj = CreateNewObject(pool.name, pool.prefab);
                ArrangePool(obj);
            }

            if (poolDictionary[pool.name].Count <= 0)
                Debug.LogError($"{pool.name}{INFO}");
            else if (poolDictionary[pool.name].Count != pool.initSize)
                Debug.LogError($"There is more than two ReturnToPool function in {pool.name}");
        }
    }

    public void DestroyPool()
    {
        foreach(var pool in pools)
        {
            List<GameObject> spawnedObjList = spawnObjects.FindAll(x => x.name == pool.name);
            foreach(var obj in spawnedObjList)
            {
                if (obj != null)
                    Destroy(obj);
            }
        }

        if (gameObject) Destroy(gameObject);
    }

    public GameObject SpawnFromPool(string name, Vector3 position, bool setActive=true) => 
        _SpawnFromPool(name, position, Quaternion.identity, setActive);
    public GameObject SpawnFromPool(string name, Vector3 position, Quaternion rotation, bool setActive=true) => 
        _SpawnFromPool(name, position, rotation, setActive);

    public T SpawnFromPool<T>(string name, Vector3 position, bool setActive=true) where T : Component
    {
        GameObject obj = _SpawnFromPool(name, position, Quaternion.identity, setActive);
        if (obj.TryGetComponent(out T component)) return component;
        else
        {
            // obj.SetActive(false);
            throw new Exception($"Component not found");
        }
    }

    public T SpawnFromPool<T>(string name, Vector3 position, Quaternion rotation, bool setActive=true) where T : Component
    {
        GameObject obj = _SpawnFromPool(name, position, rotation, setActive);
        if (obj.TryGetComponent(out T component)) return component;
        else
        {
            // obj.SetActive(false);
            throw new Exception($"Component not found");
        }
    }
    
    public List<GameObject> GetAllPools(string name)
    {
        if (!poolDictionary.ContainsKey(name))
            throw new Exception($"Pool with name {name} doesn't exist");
        
        return spawnObjects.FindAll(x => x.name == name);
    }
    public List<T> GetAllPools<T>(string name) where T : Component
    {
        List<GameObject> objects = GetAllPools(name);

        if (!objects[0].TryGetComponent(out T component))
            throw new Exception("Component not found");
        
        return objects.ConvertAll(x => x.GetComponent<T>());
    }

    protected abstract GameObject CreateNewObject(string name, GameObject prefab);

    protected abstract GameObject _SpawnFromPool(string name, Vector3 position, Quaternion rotation, bool setActive=true);

    protected void ArrangePool(GameObject obj)
    {
        bool isFind = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            // there is no same object exist in pool
            if (i == transform.childCount -1)
            {
                obj.transform.SetSiblingIndex(i);
                spawnObjects.Insert(i, obj);
                break;
            }
            // search until latest same object reach
            else if (transform.GetChild(i).name == obj.name)
                isFind = true;
            // make object locate under latest same object in hierachy
            else if (isFind)
            {
                obj.transform.SetSiblingIndex(i);
                spawnObjects.Insert(i, obj);
                break;
            }
        }
    }

    public void ReturnToPool(GameObject obj)
    {
        if (poolDictionary.ContainsKey(obj.name) ==false)
        {
            Destroy(obj);
            throw new Exception($"Pool with name {obj.name} doesn't exist");
        }

        poolDictionary[obj.name].Enqueue(obj);
        if (obj.transform.root != transform)
        SetParent(obj);
    }

    protected void SetParent(GameObject obj)
    {
        StartCoroutine(IESetParent(obj));
    }
    protected IEnumerator IESetParent(GameObject obj)
    {
        yield return null;
        if (obj == null)
        {
            Debug.LogWarning("Object is Destroyed. Instead of using Destory(obj), use obj.SetActive(false)");
            yield break;
        }
        
        obj.transform.parent = transform;
        ArrangePool(obj);
    }

}