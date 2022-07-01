using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PooledObject : MonoBehaviour
{
    private void OnDisable() {
  
        ObjectPooler.instance.ReturnToPool(gameObject);
    
    }
}
