#define test
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    public Transform centerEye;
    public Transform crossHairImage;
    private Renderer imageRenderer;
    public LayerMask screenLayer;

    private IEnumerator coroutineHolder;
    private WaitForEndOfFrame eof = new WaitForEndOfFrame();

#if test
    LineRenderer lr;
#endif

    private void Awake() {
        imageRenderer = crossHairImage.GetComponent<Renderer>();
#if test
        if (TryGetComponent<LineRenderer>(out lr) == false)
            lr = gameObject.AddComponent<LineRenderer>();
        lr.startWidth = lr.endWidth = 0.1f;
        lr.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        lr.material.color = Color.red;
        Debug.LogWarning("CrossHair is in Test mode");
        OnUseCrosshair();
#endif
    }

    public void OnUseCrosshair()
    {
        coroutineHolder = UseCrosshair();
        StartCoroutine(coroutineHolder);
    }

    public void OnDisuseCrosshair()
    {
        if (coroutineHolder != null)
            StopCoroutine(coroutineHolder);
    }

    IEnumerator UseCrosshair()
    {
        while (true)
        {
            yield return eof;
            Ray ray = new Ray(transform.position, transform.forward);
            imageRenderer.enabled = false;
#if test
            lr.SetPosition(0, ray.origin);
            lr.SetPosition(1, ray.origin + ray.direction * 100);
#endif
            if (Physics.Raycast(ray, out RaycastHit targetHit, float.MaxValue))
            {
                Vector3 targetToEye = centerEye.position - targetHit.point;
                if (Physics.Raycast(targetHit.point, targetToEye, out RaycastHit screenHit, float.MaxValue, screenLayer))
                {
                    imageRenderer.enabled = true;
                    crossHairImage.position = screenHit.point;
                    crossHairImage.forward = -targetToEye.normalized;
                    // crossHairImage.up = screenHit.normal;
                }
            }
        }
    }
}
