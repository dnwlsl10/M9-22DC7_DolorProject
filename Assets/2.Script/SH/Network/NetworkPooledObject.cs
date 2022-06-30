using UnityEngine;
using Photon.Pun;

public class NetworkPooledObject : MonoBehaviourPun
{
    private void Awake() 
    {        
        // Particle Collision Setting
        var particle = GetComponentInChildren<ParticleSystem>();
        if (particle != null) InitParticle(ref particle);

        // Collider Collision Setting
        var collider = GetComponentInChildren<Collider>();
        if (collider != null && photonView.Mine == false)
            collider.enabled = false;

        if (photonView.Mine == false)
            gameObject.SetActive(false);
    }
    void InitParticle(ref ParticleSystem particle)
    {
        var main = particle.main;
        main.loop = false;
        main.stopAction = ParticleSystemStopAction.Disable;

        var col = particle.collision;
        if (photonView.Mine)
        {
            if (col.enabled == true)
            {
                col.sendCollisionMessages = true;
                col.enableDynamicColliders = true;
                col.type = ParticleSystemCollisionType.World;
                col.quality = ParticleSystemCollisionQuality.High;
                col.mode = ParticleSystemCollisionMode.Collision3D;
                col.collidesWith = LayerMask.GetMask("RemotePlayer", "Ground");
            }
        }
        else col.enabled = false;
        
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
        if (photonView.Mine)
            NetworkObjectPool.ReturnToPool(gameObject);
    }
}
