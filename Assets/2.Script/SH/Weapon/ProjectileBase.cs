using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProjectileBase : MonoBehaviourPun
{
    public float damage;
    public float instanceNormalPositionOffset;
    public bool setParentToParentObject;
    public GameObject[] EffectsOnCollision;
    public virtual void GiveDamage(float damage){}

    protected void OnCollisionEnter(Collision other) 
    {
        if (photonView.cachedMine == false) return;
   
        var contact = other.GetContact(0);     
        if (other.collider.TryGetComponent<PhotonView>(out PhotonView pv) && pv.ViewID > 0)
        {
            photonView.CustomRPC(this, "RPCCollision", RpcTarget.AllViaServer, pv.ViewID, contact.point, contact.normal);
        }
        else
        {
            photonView.CustomRPC(this, "RPCCollision", RpcTarget.AllViaServer, 0, contact.point, contact.normal);

            if (other.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
                damageable.TakeDamage(damage);
        }
    }

    [PunRPC]
    private void RPCCollision(int viewID, Vector3 intersection, Vector3 normal)
    {

        foreach (var effect in EffectsOnCollision)
        {
            var instance = ObjectPooler.SpawnFromPool(effect, intersection + normal * instanceNormalPositionOffset, new Quaternion()) as GameObject;
            if (!setParentToParentObject) instance.transform.parent = transform;
            instance.transform.LookAt(intersection + normal);
        }

        if (viewID != 0 && PhotonNetwork.GetPhotonView(viewID).TryGetComponent<IDamageable>(out IDamageable damageable))
            if (damageable.TakeDamage(damage) == true) return;

        gameObject.SetActive(false);
    }

    [PunRPC]
    public void SelfDisable() => photonView.CustomRPC(this, "Disable", RpcTarget.All);
    private void Disable() => gameObject.SetActive(false);
}
