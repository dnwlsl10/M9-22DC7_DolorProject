using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Missile : MonoBehaviourPun
{

public Transform RocketTarget;
public Rigidbody RocketRb;

public float turnSpeed =1f;
public float rocketFlaySpeed = 10f;
private Transform roketLocalTrans;

void Start(){
    if(RocketTarget){
       Debug.Log("No Target");
    }
    roketLocalTrans = GetComponent<Transform>();
}

private void FixedUpdate(){

        if (!RocketTarget) 
            return;

        RocketRb.velocity = roketLocalTrans.forward * rocketFlaySpeed;
        var rocketTargetRot = Quaternion.LookRotation(RocketTarget.position - this.transform.localPosition);
        RocketRb.MoveRotation(Quaternion.RotateTowards(this.transform.localRotation, rocketTargetRot, turnSpeed));
}
private void OnCollisionEnter(Collision other)
 {
    if(other.gameObject.CompareTag("Enemy")){
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if(rb){
            rb.AddForceAtPosition(Vector3.up * 100f , rb.position);
        }
    }
}


//     [SerializeField]
//     private Vector3[] path;
//     private int maxPosition = 20;
//     private bool isHit;
//     private float t;
//     private int index = 0;
//     private float speed = 5;
//     public float timeSpeed = 1;
//     public GuidedMissile gm;
//     public int damage;
//     public GameObject[] effectsOnCollision;
//     public float instanceNormalPositionOffset;
//     private bool isTest;
//     public int count =1;
//     public Transform target;

//     [PunRPC]
//     public void PathRPC(Vector3 p1, Vector3 p2, Vector3 p3)
//     {
//         Debug.Log(p1);
//         Debug.Log(p2);
//         Debug.Log(p3);
//         Vector3[] path = new Vector3[maxPosition];
//         for (int i = 0; i < maxPosition; i++)
//         {
//             float t = (float)i / (maxPosition - 1);
//             path[i] = GetCurvePosition(p1, p2, p3, t);
//         }
//         SetPath(path);

//         StartCoroutine(CustomDisable());
//     }
//     IEnumerator CustomDisable()
//     {
//         yield return new WaitForSeconds(3f);
//         this.gameObject.transform.position = Vector3.zero;
//         if(this.gameObject.activeSelf) this.gameObject.SetActive(false);
//     }

//     public Vector3 GetCurvePosition(Vector3 a, Vector3 b, Vector3 c, float t)
//     {
//         Vector3 ab = Vector3.Lerp(a, b, t);
//         Vector3 bc = Vector3.Lerp(b, c, t);

//         return Vector3.Lerp(ab, bc, t);
//     }

//     public void SetPath(Vector3[] path)
//     {
//         this.path = path;
//         isTest = true;
//         for (int i = 0; i < path.Length; i++)
//         {
//             Debug.LogFormat("p{0} , {1}: ", i, path[i]);
//         }
//     }

//      private void LateUpdate()
//     {
//         if(isTest)
//         {
//             MissilePoint();
//         }
//     }

// #region  MissilePoint
//     void MissilePoint()
//     {
//         Vector3 p1 = path[index];
//         Vector3 p2;
//         if(index + 1 > path.Length - 1){
//             p2 =new Vector3(0.0562f, 0.003f, 0.0493f);
//         }
//         else{
//             p2 = path[index + 1];
//         }

//         transform.position = Vector3.Lerp(p1, p2, t);
//         Vector3 dir = p2 - p1; //진행방향
//         Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.forward);
//         transform.rotation = Quaternion.Lerp
//         (transform.rotation, targetRotation, Time.deltaTime * 5);
//         if (index < path.Length - 1)
//         {
//             t += Time.deltaTime * timeSpeed;
//             if (t > 1)
//             {
//                 index++;
//                 t = 0;
//             }
//         }
//     }
// #endregion

//     private void OnCollisionEnter(Collision other){
        
//         if(other.gameObject.CompareTag("Missile")) return;
//         if (photonView.Mine == false) return;

//         var contact = other.GetContact(0);
//         var pv = other.collider.GetComponent<PhotonView>();

//         if (pv?.ViewID > 0 == false)
//             other.collider.GetComponent<IDamageable>()?.TakeDamage(damage);
//         photonView.CustomRPC(this, "RPCCollision", RpcTarget.AllViaServer, pv?.ViewID, contact.point, contact.normal);
//     }

//     [PunRPC]
//     private void RPCCollision(int viewID, Vector3 intersection, Vector3 normal)
//     {
//         if (viewID > 0)
//             PhotonNetwork.GetPhotonView(viewID).GetComponent<IDamageable>()?.TakeDamage(damage);

//         foreach (var effect in effectsOnCollision)
//         {
//             var instance = ObjectPooler.instance.SpawnFromPool(effect, intersection + normal * instanceNormalPositionOffset, new Quaternion()) as GameObject;
//             instance.transform.LookAt(intersection + normal);
//         }

//         gameObject.SetActive(false);
//     }

//     void OnDisable()
//     {
//         if(gm !=null) gm.Destory();
//     }
}
