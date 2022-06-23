#define test
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CrossHair : MonoBehaviourPun
{
#if test
    LineRenderer lr;
#endif
    public Transform centerEye;
    public Transform crossHairImage;
    public Transform laserPoint;
    private Renderer imageRenderer;
    public LayerMask screenLayer;
    private float attackDistance;
    public float interpolationSpeed = 25;

    private IEnumerator coroutineHolder;
    private WaitForEndOfFrame eof = new WaitForEndOfFrame();

    void Reset()
    {
        if (centerEye == null)
            centerEye = Camera.main.transform;
        if (crossHairImage == null)
            crossHairImage = transform.GetChild(0);
        
        if (screenLayer == 0)
            screenLayer = LayerMask.GetMask("Grabbing");
    }

    private void Awake() {
#if test
        if (TryGetComponent<LineRenderer>(out lr) == false)
            lr = gameObject.AddComponent<LineRenderer>();
        lr.startWidth = lr.endWidth = 0.1f;
        lr.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        lr.material.color = Color.red;
        Debug.LogWarning("CrossHair is in Test mode");
#endif

        imageRenderer = crossHairImage.GetComponent<Renderer>();
        attackDistance = GetComponent<WeaponBase>().weaponSetting.attackDistance;

        if (photonView.Mine)
            OnUseCrosshair();
    }

    public void OnUseCrosshair()
    {
        imageRenderer.enabled = true;

        coroutineHolder = UseCrosshair();
        StartCoroutine(coroutineHolder);
    }

    public void OnDisuseCrosshair()
    {
        if (coroutineHolder != null)
            StopCoroutine(coroutineHolder);

        imageRenderer.enabled = false;
    }

    private Vector3 SmoothMove(Vector3 a, Vector3 b, float interpolationSpeed)
    {
        return Vector3.Lerp(a, b, Time.deltaTime * interpolationSpeed);
    }

    IEnumerator UseCrosshair()
    {
        while (true)
        {
            yield return eof;
            Ray ray = new Ray(laserPoint.position, laserPoint.forward);
            
#if test
            lr.SetPosition(0, ray.origin);
            lr.SetPosition(1, ray.origin + ray.direction * attackDistance);
#endif

            Vector3 aimPosition = Physics.Raycast(ray, out RaycastHit targetHit, attackDistance) ? targetHit.point : ray.GetPoint(attackDistance);
            Vector3 targetToEye =centerEye.position - aimPosition;

            if (Physics.Raycast(aimPosition, targetToEye, out RaycastHit screenHit, float.MaxValue, screenLayer))
            {
                imageRenderer.enabled = true;
                crossHairImage.position = SmoothMove(crossHairImage.position, screenHit.point, interpolationSpeed);
                crossHairImage.forward = -targetToEye.normalized;
                // crossHairImage.up = screenHit.normal;
            }
            else
            {
                imageRenderer.enabled = false;
            }
        }
        
    }
}
