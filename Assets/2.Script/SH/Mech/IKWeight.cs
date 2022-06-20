#define test
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using RootMotion.FinalIK;
using Photon.Pun;
public class IKWeight : MonoBehaviourPun
{
    public ButtonHandler leftGripButton;
    public ButtonHandler rightGripButton;

    IKSolverVR.Arm rightArmIK;
    IKSolverVR.Arm leftArmIK;

    IEnumerator leftIKCoroutine;
    IEnumerator rightIKCoroutine;

    // private void OnEnable()
    // {
    //     leftGripButton.OnButtonDown += OnLeftGripDown;
    //     leftGripButton.OnButtonUp += OnLeftGripUp;
    //     rightGripButton.OnButtonDown += OnRightGripDown;
    //     rightGripButton.OnButtonUp += OnRightGripUp;
    // }

    // private void OnDisable() 
    // {
    //     leftGripButton.OnButtonDown -= OnLeftGripDown;
    //     leftGripButton.OnButtonUp -= OnLeftGripUp;
    //     rightGripButton.OnButtonDown -= OnRightGripDown;
    //     rightGripButton.OnButtonUp -= OnRightGripUp;
    // }

    private void Start() 
    {
        VRIK vrIK = GetComponent<VRIK>();

        rightArmIK = vrIK.solver.rightArm;
        leftArmIK = vrIK.solver.leftArm;

        if (photonView.IsMine == false)
            GetComponent<Animator>().applyRootMotion = false;
    }

    public void SetWeightUsingRPC(bool isLeft, int targetWeight)
    {
        if (PhotonNetwork.InLobby) RPCSetWeight(isLeft, targetWeight);
        else
        {
            RPCSetWeight(isLeft, targetWeight);
            photonView.RPC("RPCSetWeight", RpcTarget.All, isLeft, targetWeight);
        }
    }

    public void OnLeftGripDown() => photonView.RPC("RPCSetWeight", RpcTarget.All, true, 1);
    public void OnRightGripDown() => photonView.RPC("RPCSetWeight", RpcTarget.All, false, 1);
    public void OnLeftGripUp() => photonView.RPC("RPCSetWeight", RpcTarget.All, true, 0);
    public void OnRightGripUp() => photonView.RPC("RPCSetWeight", RpcTarget.All, false, 0);
    
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
