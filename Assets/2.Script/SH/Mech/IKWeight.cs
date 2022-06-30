#define test
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using RootMotion.FinalIK;
using Photon.Pun;
public class IKWeight : MonoBehaviourPun
{
    #if test
    public InputActionReference gripL;
    public InputActionReference gripR;

    void Reset()
    {
#if UNITY_EDITOR
        gripL = Utility.FindInputReference(ActionMap.XRI_LeftHand_Interaction, "Select");
        gripR = Utility.FindInputReference(ActionMap.XRI_RightHand_Interaction, "Select");
#endif
    }

    void OnLeftGripEvent(InputAction.CallbackContext ctx) => OnLeftGripEvent(ctx.ReadValueAsButton() ? 1 : 0);
    void OnRightGripEvent(InputAction.CallbackContext ctx) => OnRightGripEvent(ctx.ReadValueAsButton() ? 1 : 0);

    private void OnEnable() {
        if (Utility.isVRConnected || photonView.Mine == false) return;

        gripL.action.started += OnLeftGripEvent;
        gripL.action.canceled += OnLeftGripEvent;
        gripR.action.started += OnRightGripEvent;
        gripR.action.canceled += OnRightGripEvent;
    }
    private void OnDisable() {
        if (Utility.isVRConnected || photonView.Mine == false) return;

        gripL.action.started -= OnLeftGripEvent;
        gripL.action.canceled -= OnLeftGripEvent;
        gripR.action.started -= OnRightGripEvent;
        gripR.action.canceled -= OnRightGripEvent;
    }

    #endif
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

    public void OnLeftGripEvent(int targetWeight) => photonView.CustomRPC(this, "RPCSetWeight", RpcTarget.All, true, targetWeight);
    public void OnRightGripEvent(int targetWeight) => photonView.CustomRPC(this, "RPCSetWeight", RpcTarget.All, false, targetWeight);
    
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
