using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    Transform transform;
    Vector3 prevPosition;
    RaycastHit raycastHit;
    int orbBLayer = 16;

    [Header("On Collision")]
    [SerializeField] private GameObject[] EffectsOnCollision;
    [SerializeField] private float effectNormalPositionOffset;
    [SerializeField] private bool setParent;
    [SerializeField] private float damage;
    [SerializeField] LayerMask bulletHitLayer;
    
    [Space]
    [SerializeField] private float speed;

    private void Awake() 
    {
        transform = GetComponent<Transform>();
    }

    private void OnEnable() 
    {
        prevPosition = transform.position - transform.forward * Time.deltaTime;
    }

    [PunRPC]
    private void RPCCollision(Vector3 intersection, Vector3 normal, int layer, bool showEffect)
    {
        if (showEffect)
        {
            bool isHit = true;
            if (setParent && photonView.Mine == false)
                isHit = Physics.Raycast(intersection + normal * 0.05f, -normal, out raycastHit, 0.1f, layer);

            foreach (var effect in EffectsOnCollision)
            {
                GameObject instance = ObjectPooler.instance.SpawnFromPool(effect, intersection + normal * effectNormalPositionOffset, Quaternion.identity);
                if (setParent && isHit)
                    instance.transform.parent = raycastHit.collider.transform;
                instance.transform.LookAt(intersection + normal);
            }
        }

        gameObject.SetActive(false);
    }

    private void FixedUpdate() {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (photonView.Mine == false) return;

        Vector3 dir = transform.position - prevPosition;
        if (Physics.Raycast(prevPosition, dir.normalized, out raycastHit, dir.magnitude, bulletHitLayer, QueryTriggerInteraction.UseGlobal))
        {
            raycastHit.collider.GetComponent<IDamageable>()?.TakeDamage(damage, raycastHit.point);
            photonView.CustomRPC(this, "RPCCollision", RpcTarget.All, raycastHit.point, raycastHit.normal, 1 << raycastHit.collider.gameObject.layer, raycastHit.collider.gameObject.layer != orbBLayer);
        }
        prevPosition = transform.position;
    }
}
