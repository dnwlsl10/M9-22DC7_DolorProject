using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eState{
    Normal,
    Tracking,
    TrackingComplete
}
public class GuidedMissileCrossHair : MonoBehaviour
{
    [Header("LineRenderer")]
    public Renderer imageRenderer;
    public LineRenderer lr;

    [Header("RayTarget")]
    private Vector3 origin;
    public Transform centerEye;
    private Camera cameraEye;
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
        this.cameraEye = Camera.main;
        mask = 3 << 3; 
        mask = ~mask; //미사일 레이어 제외
    }

    public void CancleGuidedMissile()
    {
        if (coroutineOnTrack != null)
        {
            StopCoroutine(coroutineOnTrack);
            coroutineOnTrack = null;
        }

        crossHairImage.gameObject.SetActive(false);
    }

    public void ActivateGuidedMissile(){

        coroutineOnTrack = StartCoroutine(OnTrack());
    }

    private void StopOnTracking(){

        if(coroutineOnTracking !=null){
            StopCoroutine(coroutineOnTracking);
            coroutineOnTracking = null;
        }
    }

   private IEnumerator OnTrack()
    {
        enemyTarget = GameObject.Find("Enemy").transform;
     
        crossHairImage.gameObject.SetActive(true);
        while (state != eState.Normal)
        {
            yield return null;
            var dir = enemyTarget.position - centerEye.position;
            var reveresDir = cameraEye.transform.position - enemyTarget.position;
            Ray ray = new Ray(centerEye.position, dir);
            Ray reveresRay = new Ray(enemyTarget.position, reveresDir);

            if (Physics.Raycast(ray, out RaycastHit targetHit , 100f , mask))
            {
                if (targetHit.transform.CompareTag("Enemy"))
                {

                    StopOnTracking();
#if UNITY_EDITOR
                    lr.SetPosition(0, ray.origin);
                    lr.SetPosition(1, targetHit.point);
#endif
                    if (Physics.Raycast(reveresRay, out RaycastHit screenHit, 100f, screenLayer))
                    {
                      
                    #if UNITY_EDITOR
                        imageRenderer.enabled = true;
                        lr.SetPosition(0, reveresRay.origin);
                        lr.SetPosition(1, screenHit.point);
#endif
                        crossHairImage.LookAt(cameraEye.transform.position);
                        crossHairImage.position = Vector3.Lerp(this.crossHairImage.position, screenHit.point, Time.deltaTime * 5f);
                        crossHairImage.rotation = Quaternion.Euler(crossHairImage.rotation.eulerAngles + new Vector3(0f, 0f, -30f));
                        Debug.Log("fireMissile");
                        state = eState.TrackingComplete;
                    }
                }
                else
                {
                    #if UNITY_EDITOR
                    lr.SetPosition(0, ray.origin);
                    lr.SetPosition(1, targetHit.point);
                    #endif
                    yield return coroutineOnTracking = StartCoroutine(OnTraking());
                }
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

