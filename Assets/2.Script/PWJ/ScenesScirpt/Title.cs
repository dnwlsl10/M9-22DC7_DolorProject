using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Title : MonoBehaviour
{
    public System.Action OnClick;
    public LoadingScreenProcess loadingAsyc;
    public GameObject txtObj;
    public AsyncOperation ao;

    private void Start()   
    {
        txtObj.gameObject.SetActive(false);

        loadingAsyc.LoadingNormalScreenProcess("Lobby" ,  (ao) =>{
            this.ao = ao;
            txtObj.gameObject.SetActive(true);
            StartCoroutine(OnAnyKeyDown());
        });       
    }

    IEnumerator OnAnyKeyDown(){
        
        while(!Input.anyKeyDown)
        {
            yield return null;
            ao.allowSceneActivation = true;   
        }
    }
}
