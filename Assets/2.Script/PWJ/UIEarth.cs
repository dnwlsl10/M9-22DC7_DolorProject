using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class UIEarth : MonoBehaviour
{

    public Text text;
    private WaitForSeconds eof = new WaitForSeconds(1f);

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
   
    [Header("Effect")]
    public GameObject effect;
    public void Init(){
        this.gameObject.SetActive(true);
        this.cm  = Camera.main;
        this.origin = this.transform.GetChild(0).transform;
        this.dir = this.origin.position - this.cm.gameObject.transform.position;
        this.screenDir =  this.origin.position - this.screenTarget.position;
    }

    public void FindOtherPlayer(System.Action OnComplete)
    {
        if (Physics.Raycast(this.cm.gameObject.transform.position, dir, out hitInfo))
        {
            if (hitInfo.collider.CompareTag("Earth"))
            {
                rotator.enabled = false;
                Debug.Log("HIt");
                this.effect.transform.position = hitInfo.point;
                this.effect.SetActive(true);
                StartCoroutine(Test());
                OnComplete();
            }
        }
    }

    IEnumerator Test(){
        text.text = "3";
        yield  return eof;
        text.text = "2";
        yield return eof;
        text.text = "1";
    }



    public void Exit() {
        this.gameObject.SetActive(false);
    }

}
