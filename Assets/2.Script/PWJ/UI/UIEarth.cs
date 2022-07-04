using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class UIEarth : MonoBehaviour
{

    [Header("Raycast Point")]
    private Transform earthCenter;
    public Transform startPos;

    [Header("Dir")]
    private Vector3 dir;
    private Vector3 screenDir;
    private RaycastHit hitInfo;

    [Header("Rotator")]
    public LocalRotator rotator;
   
    [Header("Effect")]
    public GameObject effect;
    [Header("Mat_Alpa")]
    [SerializeField] private List<Material> mats;

    public IEnumerator StartAction(Material mat){
        float val = 0f;
        mat.SetFloat("_Alpha", val);
        while(val < 1){
            yield return null;
            mat.SetFloat("_Alpha", val);
            val += 0.1f;
        }
        Debug.Log("Clean");
    }
    public void Init(){
        this.gameObject.SetActive(true);
        GetMat();
        this.earthCenter = this.transform.GetChild(0).transform;
        this.dir = this.earthCenter.position - this.startPos.transform.position;
    }

    void GetMat(){
        var renders = this.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renders.Length; i++)
        {
            StartCoroutine(StartAction(renders[i].material));
        }
    }

    public void OnRaycast()
    {
        if (Physics.Raycast(this.startPos.transform.position, dir, out hitInfo))
        {
            if (hitInfo.collider.CompareTag("Earth"))
            {
                rotator.enabled = false;
                this.effect.transform.position = hitInfo.point;
                this.effect.SetActive(true);
            }
        }
    }

    public void Exit() {
        this.gameObject.SetActive(false);
    }

}