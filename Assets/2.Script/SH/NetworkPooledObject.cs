using UnityEngine;
using Photon.Pun;

public class NetworkPooledObject : MonoBehaviourPun
{
    private void Awake() 
    {
        if (photonView.Mine == false)
            gameObject.SetActive(false);
    }
    public void Spawn(Vector3 position, Quaternion rotation)
    {
        // if (PhotonNetwork.SingleMode == false)
        //     photonView.RPC("RPCSpawn", RpcTarget.AllViaServer, position, rotation);
        // else
        //     RPCSpawn(position, rotation);
        photonView.CustomRPC(this, "RPCSpawn", RpcTarget.AllViaServer, position, rotation);
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
        if (photonView.Mine)
            NetworkObjectPool.ReturnToPool(gameObject);
    }
}
