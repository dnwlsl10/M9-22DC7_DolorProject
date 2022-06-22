using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class UIEarth : MonoBehaviour
{
    PhotonView pv;
    [Header("Raycast")]
    private Transform origin;
    private Transform target;
    private Vector3 dir;
    private RaycastHit hitInfo;
    private LocalRotator localRotator; 
    
    [Header("Effect")]
    public GameObject effect;
    public void OnEnable(){
        this.target = GameObject.FindGameObjectWithTag("Screen").transform;
        this.localRotator = this.GetComponent<LocalRotator>();
        this.dir = this.target.position - origin.position;
    }

    public void Update(){
        
        if(PhotonNetwork.CurrentRoom.PlayerCount ==2){

            if(Physics.Raycast(this.target.position ,dir , out hitInfo))
            {
                this.localRotator.enabled = false;
                if(hitInfo.collider.CompareTag("Earth")){
                    
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

    }
}
