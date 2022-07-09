using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public enum eState{
    Normal,
    Tracking,
    TrackingComplete
}
public class GuidedMissileCrossHair : MonoBehaviourPun
{
    [Header("LineRenderer")]
    public Renderer imageRenderer;
    public LineRenderer lr;

    [Header("RayTarget")]
    private Vector3 origin;
    public Transform centerEye;
    public Camera cameraEye;
    public Transform enemyTarget;
    public LayerMask screenLayer;
    public LayerMask mask;

    [Header("CrossHairImg")]
    public Transform crossHairImage;
    private bool isLook;
    public eState state;

    public Coroutine coroutineOnTrack;
    public Coroutine coroutineOnTracking;
    void Awake()
    {
        origin = centerEye.position;
        mask = 1 << 3; 
        mask = ~mask; //미사일 레이어 제외
    }

    public void StartGuidedMissile()
    {
         coroutineOnTrack = StartCoroutine(OnTrack());
    }


    public void StopGuidedMissile()
    {
        if (coroutineOnTrack != null)
        {
            StopCoroutine(coroutineOnTrack);
            coroutineOnTrack = null;
            Debug.Log("Stop Missile");
        }
        crossHairImage.gameObject.SetActive(false);
    }


    private void StopOnTracking(){

        if(coroutineOnTracking != null){
            StopCoroutine(coroutineOnTracking);
            coroutineOnTracking = null;
            Debug.Log("Stop Tracking");
        }
    }

    private IEnumerator OnTrack()
    {
        enemyTarget = GameObject.FindGameObjectWithTag("Enemy").transform;
        if (enemyTarget == null)
        {
            crossHairImage.gameObject.SetActive(false);
            yield break;
        }

        crossHairImage.gameObject.SetActive(true);
        while (true)
        {
            yield return null;
            var reveresDir = cameraEye.transform.position - enemyTarget.position;
            var distance = Vector3.Distance(cameraEye.transform.position, enemyTarget.position);
            Ray reveresRay = new Ray(enemyTarget.position, reveresDir);

            if (Physics.Raycast(reveresRay, out RaycastHit screenHit, distance, screenLayer))
            {
                if (screenHit.collider.gameObject.layer == LayerMask.NameToLayer("Screen"))
                {
                    StopOnTracking();
                    crossHairImage.LookAt(cameraEye.transform.position);
                    crossHairImage.position = Vector3.Lerp(this.crossHairImage.position, screenHit.point, Time.deltaTime * 5f);
                    crossHairImage.rotation = Quaternion.Euler(crossHairImage.rotation.eulerAngles + new Vector3(0f, 0f, -30f));
                    state = eState.TrackingComplete;
                }
                else yield return coroutineOnTracking = StartCoroutine(OnTraking());
            }
        }
    }


    private IEnumerator OnTraking()
    {
        if(state == eState.Normal) yield break;

        Vector3 direction = origin + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.2f, 0.2f), 0f);
        float floatingSpeed = Random.Range(1f, 4f);
        float rotateSpeed = Random.Range(-3f, 3f);
        while (Vector3.Distance(crossHairImage.position, direction) > 0.03f)
        {
            crossHairImage.LookAt(cameraEye.transform.position);
            crossHairImage.position = Vector3.Lerp(crossHairImage.position, direction, 0.05f);
            crossHairImage.rotation = Quaternion.Euler(crossHairImage.rotation.eulerAngles + new Vector3(0f, 0f, rotateSpeed));
            isLook = false;
         
            yield return null;
        }
    }
}

