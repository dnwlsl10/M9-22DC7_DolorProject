using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private Transform targetTransform;

    public void SetTarget(Transform target)
    {
        targetTransform = target;
    }
    void Update()
    {
        if (targetTransform != null) // 이 반경안에 적이 들어왔다면
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, targetTransform.position);
        }
    }
}
