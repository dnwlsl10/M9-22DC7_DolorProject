using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Missile : MonoBehaviourPun
{

public Transform target;
public Rigidbody missilRb;

public float turnSpeed =1f;
public float rocketFlaySpeed = 10f;
public float damage;
public GuidedMissile gm;
public GameObject[] effectsOnCollision;
public float instanceNormalPositionOffset;
public GameObject[] EffectsOnCollision;
public bool setParentToParentObject;

    [PunRPC]
    void SetTargetRPC(Transform tg)
    {
        this.target = tg;
        if(target){
            Debug.Log("No Target");
        }
       StartCoroutine(CustomDisable());
    }

    IEnumerator CustomDisable()
    {
        yield return new WaitForSeconds(10f);
        if(this.gameObject.activeSelf) this.gameObject.SetActive(false);
    }

    private void FixedUpdate(){

        if (!target) 
            return;

        if(isNot) return;
        missilRb.velocity = this.transform.forward * rocketFlaySpeed;
        var rocketTargetRot = Quaternion.LookRotation(target.position - this.transform.localPosition);
        missilRb.MoveRotation(Quaternion.RotateTowards(this.transform.localRotation, rocketTargetRot, turnSpeed));
    }

    bool isNot;
    private void OnCollisionEnter(Collision other)
    {
        if (photonView.Mine == false) return;

        var contact = other.GetContact(0);
        other.collider.GetComponent<IDamageable>()?.TakeDamage(damage);

        photonView.CustomRPC(this, "RPCCollision", RpcTarget.AllViaServer, contact.point, contact.normal);
    }
    [PunRPC]
    private void RPCCollision(int viewID, Vector3 intersection, Vector3 normal)
    {
        foreach (var effect in EffectsOnCollision)
        {
            var instance = ObjectPooler.instance.SpawnFromPool(effect, intersection + normal * instanceNormalPositionOffset, new Quaternion()) as GameObject;
            if (!setParentToParentObject) instance.transform.parent = transform;
            instance.transform.LookAt(intersection + normal);
        }
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        this.missilRb.angularVelocity = Vector3.zero;
        this.missilRb.velocity = Vector3.zero;
        if(gm !=null) gm.Destory();
    }
}
