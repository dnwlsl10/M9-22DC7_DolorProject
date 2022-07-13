using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public enum eState{
    Normal,
    Tracking,
    TrackingComplete,
    Fire
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

    public void StartGuidedMissile()
    {
        enemyTarget = GameObject.FindGameObjectWithTag("Enemy").transform;
        if(enemyTarget == null)
        {
            crossHairImage.gameObject.SetActive(false);
            return;
        }
         coroutineOnTrack = StartCoroutine(OnTrack());
    }


    public void StopGuidedMissile()
    {
        if (coroutineOnTrack != null){
            StopCoroutine(coroutineOnTrack);
            coroutineOnTrack = null;
            Debug.Log("Stop Missile");
            crossHairImage.gameObject.SetActive(false);
            StopAs();
        }
    }


    public void StopOnTracking(){

        if(coroutineOnTracking != null){
            StopCoroutine(coroutineOnTracking);
            coroutineOnTracking = null;
            Debug.Log("Stop Tracking");
            StopAs();
        }
    }

   private IEnumerator OnTrack()
    {
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
                    OnTrackCompleteSFX();
                    crossHairImage.LookAt(cameraEye.transform.position);
                    crossHairImage.position = Vector3.Lerp(this.crossHairImage.position, screenHit.point, Time.deltaTime * 5f);
                    crossHairImage.rotation = Quaternion.Euler(crossHairImage.rotation.eulerAngles + new Vector3(0f, 0f, -30f));
                    state = eState.TrackingComplete;
                }else yield return coroutineOnTracking = StartCoroutine(OnTraking());
            }
        }
    }

    


    private IEnumerator OnTraking()
    {
        if(state ==eState.Fire) yield break;
        yield return new WaitForEndOfFrame();
        
        Vector3 direction = centerEye.position;

        while (Vector3.Distance(crossHairImage.position, direction) > 0.03f)
        {
            if (state == eState.Normal) yield break;
            OnTrackingSFX();
            crossHairImage.LookAt(cameraEye.transform.position);
            crossHairImage.position = Vector3.Lerp(crossHairImage.position, centerEye.position, 0.05f);
           // crossHairImage.rotation = Quaternion.Euler(crossHairImage.rotation.eulerAngles + new Vector3(0f, 0f, rotateSpeed));
            isLook = false;
            yield return new WaitForEndOfFrame();
        }
    }

    public AudioSource ac;
    public AudioClip onTrackingSFX;
    public AudioClip onTrackCompleteSFX;
    public void OnTrackingSFX()
    {
        ac.clip = onTrackingSFX; 
        if(ac.isPlaying) return;
        ac.Play();
    }

    public void OnTrackCompleteSFX(){
        ac.clip = onTrackCompleteSFX;
        if (ac.isPlaying ) return;
        ac.Play();
    }

    public void StopAs(){
        ac?.Stop();  
    }
}

