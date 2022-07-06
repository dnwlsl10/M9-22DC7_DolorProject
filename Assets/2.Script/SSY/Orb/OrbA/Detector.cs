using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Detector : MonoBehaviourPun
{
    // 딕셔너리 - 1대1 매칭 때 사용하기 용이, 키값은 유니크해야함!!!!!(단하나와 같은)
    public GameObject linkPrefab;
    int linkIndex;
    Dictionary<Transform, int> dic;
    List<Connector> connectors = new List<Connector>();
    private void Awake() {
        GetComponentsInChildren<Connector>(true, connectors);
    }

    private void OnEnable() {
        dic = new Dictionary<Transform, int>();
    }

    void OnTriggerEnter(Collider other) //OrbA에 맞았을 때 맞은 대상 == UI에 방패불가라는 텍스트를 띄어주는
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("RemotePlayer")) //내가 아닌 나 == 상대방
        {
            Transform remotePlayerRoot = other.transform.root;
            if (dic.ContainsKey(remotePlayerRoot)) return;

            var pv = remotePlayerRoot.GetComponent<PhotonView>();
            if (pv?.ViewID > 0 == false)
                Connect(remotePlayerRoot);
            else
                photonView.CustomRPC(this, "LinkStart", RpcTarget.All, pv?.ViewID);
        }
    }

    [PunRPC]
    void LinkStart(int viewID)
    {
        PhotonView targetPv = PhotonNetwork.GetPhotonView(viewID);
        if (targetPv.Mine == true)
        {
            print("Shield Break");
            WeaponSystem.instance.LockWeapon(WeaponName.Shield);
        }

        Connect(targetPv.transform);
    }

    void Connect(Transform target)
    {
        print("Link Start");

        linkIndex = (linkIndex + 1) % connectors.Count;
        int index = linkIndex;
        connectors[index].SetTarget(target);

        dic.Add(target, index);
    }

    void OnTriggerExit(Collider other)
    {
        Transform remotePlayerRoot = other.transform.root;
        int remotePlayerLayer = LayerMask.NameToLayer("RemotePlayer");

        if (other.gameObject.layer == remotePlayerLayer) //내가 아닌 나 == 상대방
        {
            if (dic.ContainsKey(remotePlayerRoot))
            {
                var pv = remotePlayerRoot.GetComponent<PhotonView>();
                if (pv?.ViewID > 0 == false)
                    Disconnect(remotePlayerRoot, dic[remotePlayerRoot]);
                else
                    photonView.CustomRPC(this, "LinkExit", RpcTarget.All, pv?.ViewID);
            }
        }
        //로봇인지 확인
        //없는데 끊어줄 순 없음.
        //딕셔너리에서 키가 있을 때만 아래 함수를 실행한다.
    }

    [PunRPC]
    void LinkExit(int viewID=0)
    {
        PhotonView targetPv = PhotonNetwork.GetPhotonView(viewID);
        Transform remotePlayerRoot = targetPv.transform;

        if (dic.TryGetValue(remotePlayerRoot, out int index))
        {
            Disconnect(remotePlayerRoot, index);
            if (targetPv.Mine == true)
            {
                WeaponSystem.instance.UnlockWeapon(WeaponName.Shield); //네트워크 공유
            }
        }
    }

    void Disconnect(Transform tr, int index)
    {
        print("Link Exit");

        // dic.Remove(tr);
        connectors[index].Disconnect();
    }

    private void OnDisable() 
    {
        dic = null;
    }
}

