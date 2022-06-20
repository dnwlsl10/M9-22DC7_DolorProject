using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using Photon.Pun;
public class MechIKNetworkManager : MonoBehaviour
{
    [ContextMenu("Find All")]
    void FindAll()
    {
        Transform meshRoot = transform.root.Find("root").Find("mesh");
        localDisableMesh = new List<Renderer>();
        for (int i = 0; i < meshRoot.childCount; i++)
        {
            Transform child = meshRoot.GetChild(i);
            if (child.name.Equals("Arm")) continue;

            Renderer[] tmp = child.GetComponentsInChildren<Renderer>();
            localDisableMesh.AddRange(tmp);
        }

        LayerToChangeRemote = new List<GameObject>();
        Collider[] cols = transform.root.GetComponentsInChildren<Collider>();
        foreach (Collider col in cols)
            LayerToChangeRemote.Add(col.gameObject);
    }

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
        meshList = null;
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
        LayerToChangeRemote = null;
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
