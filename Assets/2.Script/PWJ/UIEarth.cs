using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class UIEarth : MonoBehaviour
{
    PhotonView pv;
    [Header("Raycast")]
    private Transform origin;
    private Transform screen;
    private Vector3 dir;
    private RaycastHit hitInfo;

    private Camera cm;
    
    [Header("Effect")]
    public GameObject effect;
    public void OnEnable(){
        this.cm  = Camera.main;
        this.origin = this.transform.GetChild(0).transform;
        this.screen = GameObject.FindGameObjectWithTag("Screen").transform;
        this.dir = this.origin.position - this.cm.gameObject.transform.position;
    }

    public void Update(){
        
        if(Input.GetKeyDown(KeyCode.J)){

            if(Physics.Raycast(this.cm.gameObject.transform.position , dir , out hitInfo))
            {
                if(hitInfo.collider.CompareTag("Earth")){
                    Debug.Log("HIt");
                    this.effect.transform.position = hitInfo.point;
                    this.effect.SetActive(true);
                }
                if(hitInfo.collider.CompareTag("Screen")){
                    SendMessage();
                }
            }
        }
    }

    void SendMessage(){
        Debug.Log("HIts");
    }
}
