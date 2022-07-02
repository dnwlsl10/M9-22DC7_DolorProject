using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Detector : MonoBehaviourPun
{
    struct study // 스트럭트 - 값을 저장하는 컨테이너와 같은 / 
    {
        public Connector connector;
        public WeaponSystem weaponSystem;
    }
    // 딕셔너리 - 1대1 매칭 때 사용하기 용이, 키값은 유니크해야함!!!!!(단하나와 같은)
    public GameObject linkPrefab;
    Dictionary<Transform, study> dic = new Dictionary<Transform, study>();

    void OnTriggerEnter(Collider other) //OrbA에 맞았을 때 맞은 대상 == UI에 방패불가라는 텍스트를 띄어주는
    {
        int remotePlayerLayer = LayerMask.NameToLayer("RemotePlayer");
        Transform remotePlayerRoot = other.transform.root;
        if (remotePlayerRoot.gameObject.layer == remotePlayerLayer) //내가 아닌 나 == 상대방
        {
            if (dic.ContainsKey(remotePlayerRoot)) return;

            print("FIND ENEMY");

            int viewID = remotePlayerRoot.GetComponent<PhotonView>().ViewID;
            if (viewID > 0) // 멀티중
            {
                photonView.CustomRPC(this, "LinkStart", RpcTarget.All, viewID);
            }
            else
            {
                print("Invalid View ID");
            }
        }
    }

    [PunRPC]
    void LinkStart(int viewID)
    {
        Transform remotePlayerRoot = PhotonNetwork.GetPhotonView(viewID).transform;
        WeaponSystem weaponSystem = remotePlayerRoot.GetComponentInChildren<WeaponSystem>();

        GameObject link = Instantiate(linkPrefab); //네트워크 공유
        link.transform.parent = this.transform;//네트워크 공유
        link.transform.localPosition = Vector3.zero; //네트워크 공유

        Connector connector = link.GetComponent<Connector>();
        connector.SetTarget(remotePlayerRoot);

        study s = new study();
        s.weaponSystem = weaponSystem;
        s.connector = connector;//네트워크 공유
        dic.Add(remotePlayerRoot, s);

        if (photonView.Mine == false)
        {
            weaponSystem.canUseSkill[(int)WeaponName.Shield] = false;
            weaponSystem.StopWeaponEvent(WeaponName.Shield);
        }
    }

    void OnTriggerExit(Collider other)
    {
        int remotePlayerLayer = LayerMask.NameToLayer("RemotePlayer");
        Transform remotePlayerRoot = other.transform.root;
        int viewID = remotePlayerRoot.GetComponent<PhotonView>().ViewID;

        if (remotePlayerRoot.gameObject.layer == remotePlayerLayer) //내가 아닌 나 == 상대방
        {
            if (dic.ContainsKey(remotePlayerRoot))
            {
                if((viewID > 0))
                photonView.CustomRPC(this, "LinkExit", RpcTarget.All, viewID);
            }
        }
        //로봇인지 확인
        //없는데 끊어줄 순 없음.
        //딕셔너리에서 키가 있을 때만 아래 함수를 실행한다.
    }

    [PunRPC]
    void LinkExit(int viewID)
    {
        Transform remotePlayerRoot = PhotonNetwork.GetPhotonView(viewID).transform;

        if (dic.TryGetValue(remotePlayerRoot, out study s))
        {

            s.weaponSystem.canUseSkill[(int)WeaponName.Shield] = true; //네트워크 공유
            Destroy(s.connector.gameObject); //네트워크 공유
        }
    }
}

