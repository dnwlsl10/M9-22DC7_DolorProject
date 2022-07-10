using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Missile : MonoBehaviourPun
{

    public Transform target;
    [SerializeField] Rigidbody missilRb;

    [SerializeField] float turnSpeed = 1f;
    [SerializeField] float rocketFlySpeed = 10f;
    public float damage;
    public GuidedMissile gm;
    [SerializeField] GameObject[] effectsOnCollision;
    [SerializeField] float instanceNormalPositionOffset;

    private void OnEnable() {
        missilRb.velocity = missilRb.angularVelocity = Vector3.zero;
    }

    [PunRPC]
    void SetTargetRPC(int viewID)
    {
        Launch(PhotonNetwork.GetPhotonView(viewID).transform);
    }

    public void Launch(Transform tr)
    {
        target = tr;
        gameObject.SetActive(true);
        //  StartCoroutine(CustomDisable());
    }

    IEnumerator CustomDisable()
    {
        yield return new WaitForSeconds(10f);
        this.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (target == null) 
            return;

        missilRb.velocity = this.transform.forward * rocketFlySpeed;
        var rocketTargetRot = Quaternion.LookRotation(target.position - this.transform.localPosition);
        missilRb.MoveRotation(Quaternion.RotateTowards(this.transform.localRotation, rocketTargetRot, turnSpeed));
    }

    bool isNot;
    private void OnCollisionEnter(Collision other)
    {
        if (photonView.Mine == false) return;

        var contact = other.GetContact(0);
        other.collider.GetComponent<IDamageable>()?.TakeDamage(damage);

        gm?.Destory();
        photonView.CustomRPC(this, "RPCCollision", RpcTarget.AllViaServer, contact.point, contact.normal);
    }
    [PunRPC]
    private void RPCCollision(Vector3 intersection, Vector3 normal)
    {
        foreach (var effect in effectsOnCollision)
        {
            GameObject instance = ObjectPooler.instance.SpawnFromPool(effect, intersection + normal * instanceNormalPositionOffset, Quaternion.identity);
            instance.transform.LookAt(intersection + normal);
        }

        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        target = null;
        // if(gm !=null) gm.Destory();
    }
}
