using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FracturedBuilding : MonoBehaviour
{
    Rigidbody[] rbs;
    int originLayer;
    private void Awake() {
        rbs = GetComponentsInChildren<Rigidbody>();
        originLayer = rbs[0].gameObject.layer;
    }
    private void OnDisable() {
        for (int i = 0; i < rbs.Length; i++)
        {
            rbs[i].velocity = rbs[i].angularVelocity = Vector3.zero;
            rbs[i].transform.localPosition = rbs[i].transform.localEulerAngles = Vector3.zero;
            rbs[i].gameObject.layer = originLayer;
        }
    }
}
