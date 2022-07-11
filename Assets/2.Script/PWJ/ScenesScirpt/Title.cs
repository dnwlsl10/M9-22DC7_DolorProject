using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Title : MonoBehaviour
{
    public System.Action OnClick;
    public LoadingScreenProcess loadingAsyc;
    public GameObject txtObj;
    public AsyncOperation ao;

    public void Init()   
    {
        txtObj.gameObject.SetActive(false);

       StartCoroutine(loadingAsyc.LoadingNormalScreenProcess("Lobby" ,  (ao) =>{
            this.ao = ao;
            txtObj.gameObject.SetActive(true);
            StartCoroutine(OnAnyKeyDown());
        }));       
    }

    IEnumerator OnAnyKeyDown(){
        
        while(!Input.anyKeyDown)
            yield return null;

        ao.allowSceneActivation = true;

        yield return new WaitForSeconds(3f);
        OnClick();
    }
}
