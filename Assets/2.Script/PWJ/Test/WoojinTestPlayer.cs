using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WoojinTestPlayer : MonoBehaviourPunCallbacks
{
     public PhotonView pv;
     private GameObject start;
     private GameObject end;

    [SerializeField]
     private MissileFire mf;
     public NetworkObjectPool objectPool;
     public Camera cm;

    public UnityEngine.InputSystem.InputActionReference alpha1;
    void Start()
    {
        this.mf = this.GetComponentInChildren<MissileFire>();
        this.start = GameObject.Find("a");
        this.end = GameObject.Find("b");
        Debug.LogFormat("isMind : {0}, Mind : {1} , SingleMode : {2}", pv.IsMine, pv.Mine, PhotonNetwork.SingleMode);
        this.transform.position = pv.Mine ? this.start.transform.position : this.end.transform.position;
        this.gameObject.name = pv.Mine ? "Player" : "Enemy";
        
        if(pv.Mine){
            this.cm.enabled = true;
        }
        if(pv.Mine) Instantiate(objectPool);
    }
}
