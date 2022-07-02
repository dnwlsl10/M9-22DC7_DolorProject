using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Photon.Pun;

public class OrbBase : MonoBehaviourPun
{
    [SerializeField]
    protected float onShootSpeed;
    protected float orbSpeed;
   

    protected void Update()
    {
        OrbMoving();
    }
    protected void OnEnable() {
        Init();
    }

    protected virtual void Init()
    {
        orbSpeed = 0;
    }
    protected void OrbMoving()
    {
        Vector3 dir = this.transform.forward;
        transform.position += dir * orbSpeed * Time.deltaTime;
    }
    
    public void OrbFire()
    {
        photonView.CustomRPC(this, "CallRpc", RpcTarget.AllViaServer, this.transform.position, this.transform.forward);
    }

    [PunRPC]
    protected void CallRpc(Vector3 shootPosition, Vector3 forward)
    {
        RPCFire(shootPosition, forward);
    }

    protected virtual void RPCFire(Vector3 shootPosition, Vector3 forward)
    {
        this.transform.position = shootPosition;
        this.transform.forward = forward;

        this.transform.SetParent(null);
        orbSpeed = onShootSpeed;
    }
}
