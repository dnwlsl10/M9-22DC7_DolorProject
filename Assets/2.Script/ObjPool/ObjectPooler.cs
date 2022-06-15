using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ObjectPooler))]
public class ObjectPoolerEditor : Editor
{
    const string INFO = "Pooled object must have\n"
    + "void OnDisable()\n"
    + "{\n"
    + "ObjectPooler.ReturnToPool(gameObject); // must called only \"once\" in one object\n"
    + "//CancelInvoke(); // if Script use Invoke function\n"
    + "}";

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(INFO, MessageType.Info);
        base.OnInspectorGUI();
    }
}
#endif

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;
    void Awake() => instance = this;

    [Serializable]
    public class Pool
    {
        public string name;
        public GameObject prefab;
        public int initSize; // init size
    }

    [SerializeField] Pool[] pools;
    List<GameObject> spawnObjects;
    Dictionary<string, Queue<GameObject>> poolDictionary;
    readonly string INFO = "Pooled object must have\n"
        + "void OnDisable()\n"
        + "{\n"
        + "ObjectPooler.ReturnToPool(gameObject); // must called only \"once\" in one object\n"
        + "//CancelInvoke(); // if Script use Invoke function\n"
        + "}";
    
    [ContextMenu("GetSpawnObjectsInfo")]
    void GetSpawnObjectsInfo()
    {
        foreach(var pool in pools)
        {
            int count = spawnObjects.FindAll(x => x.name == pool.name).Count;
            Debug.Log($"{pool.name} count : {count}");
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
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

    public static GameObject SpawnFromPool(string name, Vector3 position) => 
        instance._SpawnFromPool(name, position, Quaternion.identity);
    public static GameObject SpawnFromPool(string name, Vector3 position, Quaternion rotation) => 
        instance._SpawnFromPool(name, position, rotation);
    public static GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation) =>
        instance._SpawnFromPool(prefab, position, rotation);

    public static T SpawnFromPool<T>(string name, Vector3 position) where T : Component
    {
        GameObject obj = instance._SpawnFromPool(name, position, Quaternion.identity);
        if (obj.TryGetComponent(out T component)) return component;
        else
        {
            obj.SetActive(false);
            throw new Exception($"Component not found");
        }
    }

    public static T SpawnFromPool<T>(string name, Vector3 position, Quaternion rotation) where T : Component
    {
        GameObject obj = instance._SpawnFromPool(name, position, rotation);
        if (obj.TryGetComponent(out T component)) return component;
        else
        {
            obj.SetActive(false);
            throw new Exception($"Component not found");
        }
    }
    
    public static List<GameObject> GetAllPools(string name)
    {
        if (!instance.poolDictionary.ContainsKey(name))
            throw new Exception($"Pool with name {name} doesn't exist");
        
        return instance.spawnObjects.FindAll(x => x.name == name);
    }
    public static List<T> GetAllPools<T>(string name) where T : Component
    {
        List<GameObject> objects = GetAllPools(name);

        if (!objects[0].TryGetComponent(out T component))
            throw new Exception("Component not found");
        
        return objects.ConvertAll(x => x.GetComponent<T>());
    }

    GameObject CreateNewObject(string name, GameObject prefab)
    {
        var obj = Instantiate(prefab, transform);
        obj.name = name;
        obj.SetActive(false); // call OnDisable() --> ReturnToPool

        return obj;
    }

    GameObject _SpawnFromPool(string name, Vector3 position, Quaternion rotation)
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

    void ArrangePool(GameObject obj)
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

    public static void ReturnToPool(GameObject obj)
    {
        if (!instance.poolDictionary.ContainsKey(obj.name))
        {
            Destroy(obj);
            Debug.LogWarning("Pool with name " + obj.name + "doesn't exist // Destroy object");
            //throw new Exception($"Pool with name {obj.name} doesn't exist");
        }

        instance.poolDictionary[obj.name].Enqueue(obj);
        if (obj.transform.root != ObjectPooler.instance.transform)
        ObjectPooler.instance.SetParent(obj);
    }

    void SetParent(GameObject obj)
    {
        StartCoroutine(IESetParent(obj));
    }
    IEnumerator IESetParent(GameObject obj)
    {
        yield return null;
        obj.transform.parent = transform;
        ArrangePool(obj);
    }


}