using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;

public class NetworkObjectPoolManager : MonoBehaviourPunCallbacks
{
    public GameObject networkObjectPool;
    private GameObject spawnedPool;
    public override void OnJoinedRoom()
    {
        spawnedPool = Instantiate(networkObjectPool);
        DontDestroyOnLoad(spawnedPool);
    }

    public override void OnLeftRoom()
    {
        spawnedPool.GetComponent<NetworkObjectPool>().DestroyPool();
        GC.Collect();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        GC.Collect();
    }
}
