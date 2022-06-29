using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Missile : MonoBehaviourPun
{
    private Vector3[] path;
    private int maxPosition = 20;
    private bool isHit;
    private float t;
    private int index = 0;
    private float speed = 5;
    public float timeSpeed = 5;

    [PunRPC]
    void RPCPath(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Debug.Log(p1);
        Vector3[] path = new Vector3[maxPosition];
        for (int i = 0; i < maxPosition; i++)
        {
            float t = (float)i / (maxPosition - 1); //강제타이캐스트
            path[i] = GetCurvePosition(p1, p2, p3, t);
        }
        SetPath(path);
    }

    public Vector3 GetCurvePosition(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, t);
    }

    public void SetPath(Vector3[] path)
    {
        this.path = path;
    }

    public void Update(){
        MissilePoint();
    }

#region  MissilePoint

    void MissilePoint()
    {
        if (index >= path.Length - 1 || this.path ==null)
        {
            return;
        }
        Vector3 p1 = path[index];
        Vector3 p2 = path[index + 1];
        transform.position = Vector3.Lerp(p1, p2, t);
        Vector3 dir = p2 - p1; //진행방향
        Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.forward);
        transform.rotation = Quaternion.Lerp
        (transform.rotation, targetRotation, Time.deltaTime * 5);
        if (index < path.Length - 1)
        {
            t += Time.deltaTime * timeSpeed;
            timeSpeed += Time.deltaTime;
            if (t > 1)
            {
                index++;
                t = 0;
            }
        }
    }
#endregion

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Hit");
        if(!photonView.Mine && other.collider.tag == "Player" && other.collider.TryGetComponent<PhotonView>(out PhotonView pv) && pv.Mine){
            //other.gameObject.GetComponent<IDamageable>().TakeDamage(5f);
            Debug.Log("Hit2");
            this.gameObject.SetActive(false);
            isHit = true;
        }

        if(isHit){
            this.gameObject.SetActive(false);
        }
    }
}
