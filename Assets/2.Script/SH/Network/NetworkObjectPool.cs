using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class NetworkObjectPool : PoolBase
{
    public static NetworkObjectPool instance;
    private void Awake() {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            return;
        }
        instance = this;
        InitPool();
    }
    protected override GameObject CreateNewObject(string name, GameObject prefab)
    {
        var obj = PhotonNetwork.Instantiate("NetworkPool/"+name, Vector3.zero, Quaternion.identity);
        obj.name = name;
        obj.SetActive(false); // call OnDisable() --> ReturnToPool

        return obj;
    }

    protected override GameObject _SpawnFromPool(string name, Vector3 position, Quaternion rotation, bool setActive=true)
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
        objectToSpawn.GetComponent<NetworkPooledObject>().Spawn(position, rotation, setActive);

        return objectToSpawn;
    }

}
