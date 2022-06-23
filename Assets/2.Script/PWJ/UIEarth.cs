using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class UIEarth : MonoBehaviour
{
    PhotonView pv;
    [Header("Raycast Point")]
    private Transform origin;
    public Transform screenTarget;

    [Header("Dir")]
    private Vector3 dir;
    private Vector3 screenDir;
    private RaycastHit hitInfo;

    [Header("Rotator")]
    public LocalRotator rotator;
    private Camera cm;

    [Header("Prefab")]
    public GameObject prefab;    
   
    [Header("Effect")]
    public GameObject effect;
    public void Init(){
        this.gameObject.SetActive(true);
        this.cm  = Camera.main;
        this.origin = this.transform.GetChild(0).transform;
        this.dir = this.origin.position - this.cm.gameObject.transform.position;
        this.screenDir = this.screenTarget.position = this.origin.position;
    }

    public void FindOtherPlayer()
    {
        if (Physics.Raycast(this.cm.gameObject.transform.position, dir, out hitInfo))
        {
            if (hitInfo.collider.CompareTag("Earth"))
            {
                rotator.enabled = false;
                Debug.Log("HIt");
                this.effect.transform.position = hitInfo.point;
                this.effect.SetActive(true);
            }
        }
       
    }


    public void Exit() {
        this.gameObject.SetActive(false);
    }

}
