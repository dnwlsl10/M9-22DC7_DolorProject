using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjectPoolNetworkManager : MonoBehaviour
{
    public static ObjectPoolNetworkManager instance;
    private PhotonView pv;

    private void Awake() {
        instance = this;
        
    }
}
