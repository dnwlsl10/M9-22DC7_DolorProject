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
    [SerializeField] Transform centerEye;
    [SerializeField] Transform crossHairImage;
    [SerializeField] Transform laserPoint;
    [SerializeField] Renderer imageRenderer;
    [SerializeField] LayerMask bulletHitLayer;
    [SerializeField] LayerMask screenLayer;
    [SerializeField] private float attackDistance;
    [SerializeField] float maxInterpolationSpeed = 25;
    [SerializeField] float minInterpolationSpeed = 2;
    [Range(0, 0.1f)]
    [SerializeField] float interpolationDistance = 0.01f;

    [SerializeField] Color hitColor;
    [SerializeField] Color nonHitColor;
    [SerializeField] Material mat;
    int colorProperty;

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

        imageRenderer = crossHairImage.GetComponentInChildren<Renderer>();
        attackDistance = transform.parent.GetComponent<BasicWeapon>().weaponSetting.attackDistance;
        mat = imageRenderer.material;
        colorProperty = Shader.PropertyToID("_Color");

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
        Ray ray = new Ray(laserPoint.position, laserPoint.forward);
        bool hitTarget = Physics.Raycast(ray, out RaycastHit targetHit, attackDistance, bulletHitLayer, QueryTriggerInteraction.Ignore);

        mat.SetColor(colorProperty, hitTarget ? hitColor : nonHitColor);
        // mat.SetColor()
        Vector3 aimPosition = hitTarget ? targetHit.point : ray.GetPoint(attackDistance);
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
