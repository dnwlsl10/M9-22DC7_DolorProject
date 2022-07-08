using UnityEngine;
using Photon.Pun;

public class NetworkPooledObject : MonoBehaviourPun
{
    private void Awake() 
    {        
        // Collider Collision Setting
        if (photonView.Mine == false)
        {
            var collider = GetComponentsInChildren<Collider>();
            if (collider.Length > 0)
                for(int i = 0; i < collider.Length; i++)
                    collider[i].enabled = false;
            
            gameObject.SetActive(false);
        }            
    }

    public void Spawn(Vector3 position, Quaternion rotation, bool setActive=true)
    { 
        photonView.CustomRPC(this, "RPCSpawn", RpcTarget.AllViaServer, position, rotation, setActive); 
    }

    [PunRPC]
    private void RPCSpawn(Vector3 position, Quaternion rotation, bool setActive)
    {
        transform.position = position;
        transform.rotation = rotation;
        gameObject.SetActive(setActive);
    }

    private void OnDisable()
    {
        if (photonView.Mine) NetworkObjectPool.instance?.ReturnToPool(gameObject);
    }
}
