using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    Rigidbody rb;
    public BasicWeapon bw;
    [Header("On Collision")]
    [SerializeField] private GameObject[] EffectsOnCollision;
    [SerializeField] private float effectNormalPositionOffset;
    [SerializeField] private bool setParent;
    [SerializeField] private float damage;
    
    [Space]
    [SerializeField] private float speed;

    private void Awake() { rb = GetComponent<Rigidbody>(); }

    private void OnEnable() { rb.velocity = transform.forward * speed; }

    protected void OnCollisionEnter(Collision other) 
    {
        if (photonView.Mine == false)
        {
            print("Please Disable collider");
            return;
        }
   
        var contact = other.GetContact(0);
        other.collider.GetComponent<IDamageable>()?.TakeDamage(damage);
        bw.GetComponent<GuidedMissile>().GetGauge();
        photonView.CustomRPC(this, "RPCCollision", RpcTarget.AllViaServer, contact.point, contact.normal.normalized, 1 << other.gameObject.layer);
    }

    [PunRPC]
    private void RPCCollision(Vector3 intersection, Vector3 normal, int layer)
    {
        RaycastHit rayHit = new RaycastHit();
        bool isHit = false;
        if (setParent) isHit = Physics.Raycast(intersection + normal * 0.05f, -normal, out rayHit, 0.1f, layer);

        foreach (var effect in EffectsOnCollision)
        {
            var instance = ObjectPooler.instance.SpawnFromPool(effect, intersection + normal * effectNormalPositionOffset, new Quaternion()) as GameObject;
            if (setParent && isHit)
                instance.transform.parent = rayHit.collider.transform;
            instance.transform.LookAt(intersection + normal);
        }

        gameObject.SetActive(false);
    }
}
