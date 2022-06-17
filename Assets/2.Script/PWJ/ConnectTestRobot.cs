using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;


public class ConnectTestRobot : MonoBehaviour
{

    public List<Renderer> localDisableMesh;
    IKSolverVR.Arm rightArmIK;
    IKSolverVR.Arm leftArmIK;

    IEnumerator leftIKCoroutine;
    IEnumerator rightIKCoroutine;

    private void Start()
    {
        VRIK vrIK = GetComponent<VRIK>();

        rightArmIK = vrIK.solver.rightArm;
        leftArmIK = vrIK.solver.leftArm;

        SetLocalIKTarget();

    }

    private void DisableMesh(List<Renderer> meshList)
    {
        foreach (var mesh in meshList)
            mesh.enabled = false;
    }

    void SetLocalIKTarget()
    {
        DisableMesh(localDisableMesh);

        SetIKWeight(true, 0);
        SetIKWeight(false, 0);
    }


    public void SetIKWeight(bool isLeft, float _value)
    {
        var armIK = isLeft ? leftArmIK : rightArmIK;
        armIK.positionWeight = armIK.rotationWeight = _value;
    }
}

