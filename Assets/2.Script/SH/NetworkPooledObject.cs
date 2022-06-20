using UnityEngine;
using Photon.Pun;

public class NetworkPooledObject : MonoBehaviourPun
{
    private void Awake() 
    {
        if (photonView.IsMine == false)
            gameObject.SetActive(false);
    }
    public void Spawn(Vector3 position, Quaternion rotation)
    {
        photonView.RPC("RPCSpawn", RpcTarget.AllViaServer, position, rotation);
    }

    [PunRPC]
    private void RPCSpawn(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if (photonView.IsMine)
            NetworkObjectPool.ReturnToPool(gameObject);
    }
}
