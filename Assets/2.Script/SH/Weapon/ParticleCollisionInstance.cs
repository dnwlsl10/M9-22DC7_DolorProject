/*This script created by using docs.unity3d.com/ScriptReference/MonoBehaviour.OnParticleCollision.html*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

public class ParticleCollisionInstance : MonoBehaviourPun
{
    public float damage = 1;
    public GameObject[] EffectsOnCollision;
    public float DestroyTimeDelay = 5;
    public bool UseWorldSpacePosition;
    public float Offset = 0;
    public Vector3 rotationOffset = new Vector3(0,0,0);
    public bool useOnlyRotationOffset = true;
    public bool UseFirePointRotation;
    public bool DestoyMainEffect = true;
    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    private ParticleSystem ps;

    void Awake()
    {
        part = GetComponent<ParticleSystem>();
    }
    void OnParticleCollision(GameObject other)
    {
        if (photonView.cachedMine == false) return;
   
        if (part.GetCollisionEvents(other, collisionEvents) > 0)
        {
           if (collisionEvents[0].colliderComponent.TryGetComponent<PhotonView>(out PhotonView pv) && pv.ViewID > 0)
            {
                photonView.CustomRPC(this, "RPCCollision", RpcTarget.AllViaServer, pv.ViewID, collisionEvents[0].intersection, collisionEvents[0].normal);
            }
           else
            {
                photonView.CustomRPC(this, "RPCCollision", RpcTarget.AllViaServer, 0, collisionEvents[0].intersection, collisionEvents[0].normal);

                if (collisionEvents[0].colliderComponent.TryGetComponent<IDamageable>(out IDamageable damageable))
                    damageable.TakeDamage(damage);
            }
        }
    }

    [PunRPC]
    private void RPCCollision(int viewID, Vector3 intersection, Vector3 normal)
    {
        if (viewID != 0 && PhotonNetwork.GetPhotonView(viewID).TryGetComponent<IDamageable>(out IDamageable damageable))
            damageable.TakeDamage(damage);

        foreach (var effect in EffectsOnCollision)
        {
            var instance = ObjectPooler.SpawnFromPool(effect, intersection + normal * Offset, new Quaternion()) as GameObject;
            if (!UseWorldSpacePosition) instance.transform.parent = transform;
            if (UseFirePointRotation) { instance.transform.LookAt(transform.position); }
            else if (rotationOffset != Vector3.zero && useOnlyRotationOffset) { instance.transform.rotation = Quaternion.Euler(rotationOffset); }
            else
            {
                instance.transform.LookAt(intersection + normal);
                instance.transform.rotation *= Quaternion.Euler(rotationOffset);
            }
        }

        if (DestoyMainEffect == true)
            gameObject.SetActive(false);
    }
}
