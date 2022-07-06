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

    protected void OnCollisionEnter(Collision other) 
    {
        if (photonView.Mine == false) return;
   
        var contact = other.GetContact(0);
        other.collider.GetComponent<IDamageable>()?.TakeDamage(damage); 

        photonView.CustomRPC(this, "RPCCollision", RpcTarget.AllViaServer, contact.point, contact.normal);
    }

    [PunRPC]
    private void RPCCollision(Vector3 intersection, Vector3 normal)
    {
        foreach (var effect in EffectsOnCollision)
        {
            var instance = ObjectPooler.instance.SpawnFromPool(effect, intersection + normal * instanceNormalPositionOffset, new Quaternion()) as GameObject;
            if (!setParentToParentObject) instance.transform.parent = transform;
            instance.transform.LookAt(intersection + normal);
        }

        gameObject.SetActive(false);
    }
}
