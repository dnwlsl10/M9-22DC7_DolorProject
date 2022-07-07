using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRotation : MonoBehaviour //타겟의 축을 기준으로 회전
{
    public Transform target;
    public float rotationSpeed = 2;

    void Update()
    {
        transform.RotateAround(target.position, Vector3.up, 1 * Time.deltaTime * rotationSpeed);
    }

  
}
