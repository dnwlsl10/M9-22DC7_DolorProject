using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CrossHair : MonoBehaviour
{
    LineRenderer lr;

    public Transform centerEye;
    public Transform crossHairImage;
    public LayerMask screenLayer;


    private void Awake() {
        lr = GetComponent<LineRenderer>();
    }


    private void Update() {
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + transform.forward * 100);

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit targetHit, float.MaxValue))
        {
            Vector3 screenToEye = centerEye.position - targetHit.point;
            if (Physics.Raycast(targetHit.point, screenToEye, out RaycastHit screenHit, float.MaxValue, screenLayer))
            {
                // crossHairImage.up = screenHit.normal;
                crossHairImage.forward = -screenToEye.normalized;
                crossHairImage.position = screenHit.point;
            }
        }
    }
}
