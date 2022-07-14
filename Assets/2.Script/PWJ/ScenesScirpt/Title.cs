using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Title : MonoBehaviour
{
    public System.Action OnClick;
    public LoadingScreenProcess loadingAsyc;
    public GameObject txtObj;
    public GameObject blackBg;
    //public AsyncOperation ao;
    private WaitForEndOfFrame of = new WaitForEndOfFrame();
    private WaitForSeconds os = new WaitForSeconds(9f);
    public void Init() 
    {
        StartCoroutine(EndTitleAni());
    }

    IEnumerator EndTitleAni()
    {
        yield return os;
        StartCoroutine(OnClickAnyKey());
    }

    IEnumerator OnClickAnyKey(){

        while(!Input.anyKey)
            yield return null;

        yield return of;
        blackBg.gameObject.SetActive(true);

        StartCoroutine(loadingAsyc.LoadingNormalScreenProcess(("Lobby"), (ao) =>{

            ao.completed += (obj) =>
            {
               this.OnClick();
            };
            
            ao.allowSceneActivation = true;
        }));
    }
}
