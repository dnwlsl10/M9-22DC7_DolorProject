using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using Photon.Pun;

public class MechIKNetworkManager : MonoBehaviour
{
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

        if (pv.IsMine)
            SetLocalIKTarget();
        else
        {
            SetRemoteIKTarget();
            GetComponent<Animator>().applyRootMotion = false;
        }
    }

    void SetLocalIKTarget()
    {
        SetIKWeight(true, 0);
        SetIKWeight(false, 0);
    }
    void SetRemoteIKTarget()
    {
        leftArmIK.target.GetComponent<Rigidbody>().isKinematic = true;
        rightArmIK.target.GetComponent<Rigidbody>().isKinematic = true;

        SetIKWeight(true, 0);
        SetIKWeight(false, 0);
    }

    public void SetWeightUsingRPC(bool isLeft, int targetWeight)
    {
        pv.RPC("RPCSetWeight", RpcTarget.All, isLeft, targetWeight);

        // RPCSetWeight(isLeft, targetWeight);
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
