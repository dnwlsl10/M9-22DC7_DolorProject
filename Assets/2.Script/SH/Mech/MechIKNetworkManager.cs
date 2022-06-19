using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using Photon.Pun;
public class MechIKNetworkManager : MonoBehaviour
{
    [ContextMenu("Toggle Mesh")]
    void ToggleMesh()
    {
        foreach (var renderer in localDisableMesh)
            renderer.enabled = !renderer.enabled;
    }
    public List<Renderer> localDisableMesh;
    public List<GameObject> LayerToChangeRemote;
    IKSolverVR.Arm rightArmIK;
    IKSolverVR.Arm leftArmIK;
    PhotonView pv;

    IEnumerator leftIKCoroutine;
    IEnumerator rightIKCoroutine;

    private void Start() 
    {
        VRIK vrIK = GetComponent<VRIK>();
        pv = GetComponent<PhotonView>();

        rightArmIK = vrIK.solver.rightArm;
        leftArmIK = vrIK.solver.leftArm;

        if (pv.IsMine || PhotonNetwork.InLobby)
            SetLocal();
        else
        {
            SetRemote();
            GetComponent<Animator>().applyRootMotion = false;
        }
    }

    private void DisableMesh(List<Renderer> meshList)
    {
        foreach (var mesh in meshList)
            mesh.enabled = false;
        meshList.Clear();
    }

    void SetLocal()
    {
        DisableMesh(localDisableMesh);

        SetIKWeight(true, 0);
        SetIKWeight(false, 0);
    }
    void SetRemote()
    {
        ChangeRemoteLayer();

        SetIKWeight(true, 0);
        SetIKWeight(false, 0);
    }
    void ChangeRemoteLayer()
    {
        foreach (var obj in LayerToChangeRemote)
            obj.layer = LayerMask.NameToLayer("RemotePlayer");
        LayerToChangeRemote.Clear();
    }

    public void SetWeightUsingRPC(bool isLeft, int targetWeight)
    {
        if (PhotonNetwork.InLobby) SetIKWeight(isLeft, targetWeight);
        else
        {
            RPCSetWeight(isLeft, targetWeight);
            pv.RPC("RPCSetWeight", RpcTarget.All, isLeft, targetWeight);
        }
    }
    
    [PunRPC]
    private void RPCSetWeight(bool isLeft, int targetWeight)
    {
        print("RPC");
        if (isLeft)
        {
            if (leftIKCoroutine != null)
                StopCoroutine(leftIKCoroutine);
            leftIKCoroutine = IESetIKWeight(isLeft, targetWeight);
            StartCoroutine(leftIKCoroutine);
        }
        else
        {
            if (rightIKCoroutine != null)
                StopCoroutine(rightIKCoroutine);
            rightIKCoroutine = IESetIKWeight(isLeft, targetWeight);
            StartCoroutine(rightIKCoroutine);
        }
    }

    IEnumerator IESetIKWeight(bool isLeft, int targetWeight)
    {
        // disable hand renderer when using robot ik system
        // if (characterHandMesh != null)
        //     characterHandMesh.enabled = !increasing;

        var armIK = isLeft ? leftArmIK : rightArmIK;

        for (float f = 0; f < 1; f += Time.deltaTime)
        {
            armIK.positionWeight = armIK.rotationWeight = Mathf.Lerp(armIK.positionWeight, targetWeight, f);
            yield return null;
        }
        armIK.positionWeight = armIK.rotationWeight = targetWeight;
    }

    public void SetIKWeight(bool isLeft, float _value)
    {
        var armIK = isLeft ? leftArmIK : rightArmIK;
        armIK.positionWeight = armIK.rotationWeight = _value;
    }
}
