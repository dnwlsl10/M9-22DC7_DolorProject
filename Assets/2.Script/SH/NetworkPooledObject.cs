using UnityEngine;
using Photon.Pun;

public class NetworkPooledObject : MonoBehaviourPun
{
    private void Awake() 
    {
        if (photonView.Mine == false)
            gameObject.SetActive(false);
        
        // Particle Collision Setting
        var particle = GetComponentInChildren<ParticleSystem>();
        if (particle != null)
        {
            var main = particle.main;
            main.loop = false;
            main.stopAction = ParticleSystemStopAction.Disable;

            var col = particle.collision;
            if (photonView.cachedMine)
            {
                if (col.enabled == true)
                {
                    col.quality = ParticleSystemCollisionQuality.High;
                    col.collidesWith = LayerMask.GetMask("RemotePlayer", "Ground");
                    col.enableDynamicColliders = true;
                    col.sendCollisionMessages = true;
                    col.type = ParticleSystemCollisionType.World;
                    col.mode = ParticleSystemCollisionMode.Collision3D;
                }
            }
            else
            {
                col.enabled = false;
            }
        }

        // Collider Collision Setting
        var collider = GetComponentInChildren<Collider>();
        if (collider != null && photonView.cachedMine == false)
            collider.enabled = false;
    }
    public void Spawn(Vector3 position, Quaternion rotation)
    {
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
