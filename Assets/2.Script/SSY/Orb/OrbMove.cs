using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbMove : MonoBehaviour
{
    public float orbSpeed = 2;

    void Update()
    {
        OrbMoving();
    }
    void OrbMoving()
    {
        Vector3 dir = Vector3.forward;
        transform.position += dir * orbSpeed * Time.deltaTime;
    }
}
