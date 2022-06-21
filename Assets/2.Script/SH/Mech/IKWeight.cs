#define test
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using RootMotion.FinalIK;
using Photon.Pun;
public class IKWeight : MonoBehaviourPun
{
    IKSolverVR.Arm rightArmIK;
    IKSolverVR.Arm leftArmIK;

    IEnumerator leftIKCoroutine;
    IEnumerator rightIKCoroutine;

    private void Start() 
    {
        VRIK vrIK = GetComponent<VRIK>();

        rightArmIK = vrIK.solver.rightArm;
        leftArmIK = vrIK.solver.leftArm;
    }

    public void OnLeftGripEvent(int targetWeight)
    {
        if (PhotonNetwork.SingleMode)
            RPCSetWeight(true, targetWeight);
        else
            photonView.RPC("RPCSetWeight", RpcTarget.All, true, targetWeight);
    }
    public void OnRightGripEvent(int targetWeight)
    {
        if (PhotonNetwork.SingleMode)
            RPCSetWeight(false, targetWeight);
        else
            photonView.RPC("RPCSetWeight", RpcTarget.All, false, targetWeight);
    }
    
    [PunRPC]
    private void RPCSetWeight(bool isLeft, int targetWeight)
    {
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
}
