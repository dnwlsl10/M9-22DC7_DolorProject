#define test
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CrossHair : MonoBehaviourPun, IInitialize
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
    public float maxInterpolationSpeed = 25;
    public float minInterpolationSpeed = 2;
    [Range(0, 0.1f)]
    public float interpolationDistance = 0.01f;

    [ContextMenu("Initialize")]
    public void Initialize(){
        if(centerEye == null){
            centerEye = transform.root.GetComponentInChildren<Camera>(true)?.transform;
        }
    }
    public void Reset()
    {
        if (centerEye == null)
            centerEye = transform.root.GetComponentInChildren<Camera>(true)?.transform;
        if (crossHairImage == null)
            crossHairImage = transform.GetChild(0);
        if (laserPoint == null)
            laserPoint = GetComponent<BasicWeapon>().bulletSpawnPoint;
        if (screenLayer == 0)
            screenLayer = LayerMask.GetMask("Screen");
    }

    private void Awake() 
    {
        if (photonView.Mine == false)
        {
            Destroy(this);
            return;
        }

        imageRenderer = crossHairImage.GetComponent<Renderer>();
        attackDistance = transform.parent.GetComponent<BasicWeapon>().weaponSetting.attackDistance;

        #if UNITY_EDITOR && test
            if (TryGetComponent<LineRenderer>(out lr) == false)
                lr = gameObject.AddComponent<LineRenderer>();
            lr.startWidth = lr.endWidth = 0.1f;
            lr.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            lr.material.color = Color.red;
            Debug.LogWarning("CrossHair is in Test mode");
        #endif
    }

    private Vector3 SmoothMove(Vector3 a, Vector3 b)
    {
        float distance = Vector3.Distance(a, b);
        return Vector3.Lerp(a, b, Mathf.Clamp(distance/interpolationDistance*maxInterpolationSpeed, minInterpolationSpeed, maxInterpolationSpeed) * Time.deltaTime);
    }

    private void Update() {
        Ray ray = new Ray(laserPoint.position + laserPoint.forward, laserPoint.forward);
            
        Vector3 aimPosition = Physics.Raycast(ray, out RaycastHit targetHit, attackDistance) ? targetHit.point : ray.GetPoint(attackDistance);
        Vector3 targetToEye = centerEye.position - aimPosition;
        if (Physics.Raycast(aimPosition, targetToEye, out RaycastHit screenHit, targetToEye.magnitude, screenLayer) && screenHit.collider.gameObject.layer == LayerMask.NameToLayer("Screen"))
        {
            imageRenderer.enabled = true;
            crossHairImage.position = SmoothMove(crossHairImage.position, screenHit.point);
            crossHairImage.forward = -targetToEye.normalized;
            // crossHairImage.up = screenHit.normal;
        }
        else imageRenderer.enabled = false;

        #if UNITY_EDITOR && test
            lr.SetPosition(0, ray.origin);
            lr.SetPosition(1, aimPosition);
        #endif
    }
}
