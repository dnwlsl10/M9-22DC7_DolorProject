
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MissileFire : MonoBehaviourPun
{
    private GameObject target;
    public Transform firePosition;

    public Transform[] randomPath;
    int count; // 발사갯수


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        count = 20;
        if (photonView.Mine)
            MakeMissile();
    }

   void MakeMissile()
    {
        this.target = GameObject.Find("Enemy");
        int randIndex = Random.Range(0, randomPath.Length - 1);
        Vector3 dir = new Vector3
            (
            Random.Range(-1f, 1f),
            Random.Range(0.1f, 1f),
            0
            );
        dir.Normalize();
        // dir *= 3.85f;// Y값
        dir += new Vector3(0, 0, -8.25f); // Z값

        Vector3 p1 = firePosition.transform.position;
        Vector3 p2 = new Vector3(randomPath[randIndex].position.x, randomPath[randIndex].position.y, 0) + dir;
        Vector3 p3 = target.transform.position;
        
        count--;//인보크로 발사갯수를 차감하고 카운트가 0과 같아지면 리턴. 그전까지는 발사시간(0.2f)마다 생성해서 발사해준다.
        Debug.Log("Test");
        GameObject missile = NetworkObjectPool.SpawnFromPool("DolorMissileKey2", firePosition.transform.position, Quaternion.identity);
        missile.transform.position = firePosition.transform.position;
        // 로켓에게 커브에 관련된 점배열을 알려주고싶다.
        Missile m = missile.GetComponent<Missile>();
        var mPv = m.GetComponent<PhotonView>();
        mPv.RPC("RPCPath", RpcTarget.All, p1, p2, p3);
    }
}
