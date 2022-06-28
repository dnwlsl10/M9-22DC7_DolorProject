using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class VRIK_Limit : MonoBehaviour
{
    VRIK ik;
    public RotationLimit[] rotationLimits;
    public bool useLimit;
    private void Awake() 
    {
        ik = transform.root.GetComponent<VRIK>();
        rotationLimits = transform.root.GetComponentsInChildren<RotationLimit>(true);
    }
    private void OnEnable() {
        if (useLimit == false) return;

        foreach (RotationLimit limit in rotationLimits)
            limit.enabled = false;
        ik.solver.OnPostUpdate += AfterVRIK;
    }

    private void OnDisable() {
        var eventList = ik.solver.OnPostUpdate?.GetInvocationList();
        if (eventList == null) return;
        foreach (var a in eventList)
            if (a.Method.Name.Equals("AfterVRIK"))
                ik.solver.OnPostUpdate -= AfterVRIK;
    }

    private void AfterVRIK()
    {
        foreach (RotationLimit limit in rotationLimits)
            limit.Apply();
    }
}
